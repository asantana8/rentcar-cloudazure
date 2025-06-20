using System;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace fnPayment;

public class Payment
{
    private readonly ILogger<Payment> _logger;
    private readonly IConfiguration _configuration;
    private readonly string[] StatusList = { "Aprovado", "Reprovado", "Em análise" };
    private readonly Random random = new();

    public Payment(ILogger<Payment> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    [Function(nameof(Payment))]
    [CosmosDBOutput("%CosmosDB%", "%CosmosContainer%", Connection = "CosmosDBConnection", CreateIfNotExists = false)]
    public async Task<object?> Run(
        [ServiceBusTrigger("payment-queue", Connection = "ServiceBusConnection")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        // Iniciar uma classe para carregar o PaymentModel
        PaymentModel payment = null;

        try
        {
            payment = JsonSerializer.Deserialize<PaymentModel>(message.Body.ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (payment == null)
            {
                await messageActions.DeadLetterMessageAsync(message, null, "The message could not be deserialized.");
                return null; // Ensure a return value here
            }

            int index = random.Next(0, StatusList.Length);
            string status = StatusList[index];
            payment.status = status;

            if (status == "Aprovado")
            {
                payment.dataAprovacao = DateTime.Now;
                await SendToNotificationQueue(payment);
            }

            return payment; // Return the processed payment object
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro 500");
            await messageActions.DeadLetterMessageAsync(message, null, $"Erro:{ex.Message}");
            return null; // Ensure a return value here
        }
        finally
        {
            // Complete the message
            await messageActions.CompleteMessageAsync(message);
        }
    }

    private async Task SendToNotificationQueue(PaymentModel payment)
    {
        var connectionString = _configuration.GetSection("ServiceBusConnection").Value.ToString();
        var queeuName = _configuration.GetSection("NotificationQueue").Value.ToString();

        var serviceBusClient = new ServiceBusClient(connectionString);
        var sender = serviceBusClient.CreateSender(queeuName);
        var message = new ServiceBusMessage(JsonSerializer.Serialize(payment))
        {
            ContentType = "application;json",
            MessageId = payment.idPayment.ToString()
        };

        message.ApplicationProperties["idPayment"] = payment.idPayment.ToString();
        message.ApplicationProperties["type"] = "notification";
        message.ApplicationProperties["message"] = "Pagamento Aprovado com Sucesso.";

        try
        {
            await sender.SendMessageAsync(message);
            _logger.LogInformation("Message send to notification queue: {id}", payment.id.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending message to notification queue");
        }
        finally
        {
            await sender.DisposeAsync();
            await serviceBusClient.DisposeAsync();
        }

    }
}