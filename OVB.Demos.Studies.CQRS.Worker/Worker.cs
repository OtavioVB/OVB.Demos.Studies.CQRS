using MySql.Data.MySqlClient;
using OVB.Demos.Studies.CQRS.Domain;
using ProtoBuf;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Data.SQLite;
using System.Security.Policy;
using System.Text;

namespace OVB.Demos.Studies.CQRS.Worker;

public class Worker : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
        };
        
        using (var connectionRbmq = factory.CreateConnection())
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var channel = connectionRbmq.CreateModel())
                {
                    channel.QueueDeclare(
                    queue: "CQRS_Synchronism", // nome da fila
                    durable: true, // permitir a fila permanecer ativa após o servidor ser reiniciado
                    exclusive: false, // acessar apenas pela conexão atual
                    autoDelete: true, // deletar automaticamente após a fila ser consumida
                    arguments: null);


                    var consumer = new EventingBasicConsumer(channel); // Solicitação da entrada das mensagens de forma assíncrona
                    
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        using var readonlyMemory = new MemoryStream(body);
                        var account = Serializer.Deserialize<Account>(readonlyMemory);

                        SQLiteConnection connection = new SQLiteConnection($"Data source={Path.Combine(AppContext.BaseDirectory, "cqrs.db")}");
                        connection.Open();

                        var command = connection.CreateCommand();

                        command.CommandText = "INSERT INTO Accounts (Identifier, Name) VALUES (@Identifier, @Name)";
                        command.Parameters.AddWithValue("@Identifier", account.Identifier);
                        command.Parameters.AddWithValue("@Name", account.Name);
                        command.ExecuteNonQuery();

                    }; // recebe a mensagem da fila converte para string e imprime no console

                    channel.BasicConsume(queue: "CQRS_Synchronism", autoAck: true, consumer: consumer);
                    Console.WriteLine(DateTime.UtcNow.ToString());

                }

                await Task.Delay(1000);
            }    
       }
    }
}