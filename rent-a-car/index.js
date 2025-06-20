const express = require('express');
const cors = require('cors');
const {DefaulAzureCredential} = require('@azure/identity');
const {ServiceBusClient} = require('@azure/service-bus');
require('dotenv').config();
const app = express();
app.use(cors());
app.use(express.json());

app.post('/api/locacao', async (req, res) => {
    const {nome, email, modelo, ano, tempoAluguel} = req.body;
    if (!nome || !email) {
        return res.status(400).json({error: 'Nome e email são obrigatórios.'});
    }
    const connectionString = process.env.SERVICE_BUS_CONNECTION_STRING;
    if (!connectionString) {
        return res.status(500).json({error: 'Configuração do Service Bus não encontrada.'});
    }
      
    const mensagem = {
        nome,
        email,
        modelo,
        ano,
        tempoAluguel,
        data: new Date().toISOString()
    };

    try{
        //const credential = new DefaulAzureCredential();
        const serviceBusConnection = connectionString;
        const queueName = 'fila-locacao-auto';
        const sbClient = new ServiceBusClient(serviceBusConnection);
        const sender = sbClient.createSender(queueName);
        const message = {
            body: mensagem,
            contentType: 'application/json',
            subject: 'Locação de Veículo'
        };

        await sender.sendMessages(message);
        await sender.close();
        await sbClient.close();

        console.log('Mensagem enviada com sucesso:', mensagem);
        return res.status(200).json({message: 'Locação de veículo enviada para fila com sucesso.'});

    }catch (error) {
        console.error('Erro ao enviar mensagem:', error);
        return res.status(500).json({error: 'Erro ao enviar mensagem.'});
    }

});

app.listen(3001, () => {
    console.log('Servidor rodando na porta 3001');
});