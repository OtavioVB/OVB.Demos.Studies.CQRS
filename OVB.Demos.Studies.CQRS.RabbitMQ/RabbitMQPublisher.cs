using RabbitMQ.Client;
using System.Text;

namespace OVB.Demos.Studies.CQRS.RabbitMQ;

public class RabbitMQPublisher
{
    public void Public(byte[] message)
    {
        Console.WriteLine(DateTime.UtcNow.ToString());
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
        };

        using (var connection = factory.CreateConnection())
        {
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "CQRS_Synchronism", // nome da fila
                    durable: true, // permitir a fila permanecer ativa após o servidor ser reiniciado
                    exclusive: false, // acessar apenas pela conexão atual
                    autoDelete: true, // deletar automaticamente após a fila ser consumida
                    arguments: null);
                channel.BasicPublish(exchange: "", routingKey: "CQRS_Synchronism", basicProperties: null, body: message);
            }
        }
    }
}
