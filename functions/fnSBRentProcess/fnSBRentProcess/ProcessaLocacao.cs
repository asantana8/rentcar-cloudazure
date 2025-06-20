using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Ini;
using Microsoft.Extensions.Logging;

namespace fnSBRentProcess;

public class ProcessaLocacao
{
    private readonly ILogger<ProcessaLocacao> _logger;
    private readonly IConfiguration _configuration;

    public ProcessaLocacao(ILogger<ProcessaLocacao> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    [Function(nameof(ProcessaLocacao))]
    public async Task Run(
        [ServiceBusTrigger("fila-locacao-auto", Connection = "ServiceBusConnection")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        var body = message.Body.ToString();
        _logger.LogInformation("Message Body: {body}", body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        RentModel rentModel = null;
        try
        {
            rentModel = System.Text.Json.JsonSerializer.Deserialize<RentModel>(body, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (rentModel is null)
            {
                _logger.LogError("RentModel mal formatada. Message body: {body}", body);
                await messageActions.DeadLetterMessageAsync(message, null, "Mensagem mal formatada");
            }

            var connectionString = _configuration.GetConnectionString("SqlConnection");
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Locacao (Nome, Email, Modelo, Ano, TempoAluguel, Data) VALUES (@Nome, @Email, @Modelo, @Ano, @TempoAluguel, @Data)";
                command.Parameters.AddWithValue("@Nome", rentModel.nome);
                command.Parameters.AddWithValue("@Email", rentModel.email);
                command.Parameters.AddWithValue("@Modelo", rentModel.modelo);
                command.Parameters.AddWithValue("@Ano", rentModel.ano);
                command.Parameters.AddWithValue("@TempoAluguel", rentModel.tempoAluguel);
                command.Parameters.AddWithValue("@Data", rentModel.data);

                //antes de executar o comando, enviar mensagem para fila de pagamento
                var serviceBusConnectio = _configuration.GetSection("ServiceBusConnection").Value.ToString();
                var serviceBusQueue = _configuration.GetSection("ServiceBusQueue").Value.ToString();
                sendMessageToPay(serviceBusConnectio, serviceBusQueue, rentModel);

                var rowsAffected = await command.ExecuteNonQueryAsync();
            }

            // Complete the message
            await messageActions.CompleteMessageAsync(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar mensagem");
            await messageActions.DeadLetterMessageAsync(message, null, "Erro ao processar mensagem:" + ex.Message);
            return;
        }


    }

    private void sendMessageToPay(string serviceBusConnectio, string serviceBusQueue, RentModel rentModel)
    {
        ServiceBusClient serviceBusClient = new(serviceBusConnectio);
        ServiceBusSender serviceBusSender = serviceBusClient.CreateSender(serviceBusQueue);
        ServiceBusMessage message = new(System.Text.Json.JsonSerializer.Serialize(rentModel, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }))
        {
            ContentType = "application/json"
        };
        message.ContentType = "application/json";
        message.ApplicationProperties.Add("tipo", "Pagamento");
        message.ApplicationProperties.Add("nome", rentModel.nome);
        message.ApplicationProperties.Add("email", rentModel.email);
        message.ApplicationProperties.Add("modelo", rentModel.modelo);
        message.ApplicationProperties.Add("ano", rentModel.ano.ToString());
        message.ApplicationProperties.Add("tempoAluguel", rentModel.tempoAluguel);
        message.ApplicationProperties.Add("data", rentModel.data.ToString("o")); // ISO 8601 format
        _logger.LogInformation("Enviando mensagem para fila de pagamento: {message}", message.Body.ToString());
        serviceBusSender.SendMessageAsync(message).GetAwaiter().GetResult();
        _logger.LogInformation("Mensagem enviada para fila de pagamento com sucesso: {messageId}", message.MessageId);
        serviceBusSender.DisposeAsync().GetAwaiter().GetResult();

    }
}