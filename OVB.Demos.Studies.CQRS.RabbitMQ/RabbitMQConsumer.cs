using RabbitMQ.Client.Events;
using RabbitMQ.Client;

namespace OVB.Demos.Studies.CQRS.RabbitMQ;

public class RabbitMQConsumer
{
    public byte[]? Consume()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
        };

        using (var connection = factory.CreateConnection())
        {
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "CQRS_Synchronism",
                    durable: false,
                    exclusive: false,
                    autoDelete: true,
                    arguments: null);


                var consumer = new EventingBasicConsumer(channel); // Solicitação da entrada das mensagens de forma assíncrona

                byte[]? message = null;

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    message = body;
                }; // recebe a mensagem da fila converte para string e imprime no console

                channel.BasicConsume(queue: "CQRS_Synchronism", autoAck: true, consumer: consumer);

                return message;
            }
        }
    }
}
