using MySql.Data.MySqlClient;
using OVB.Demos.Studies.CQRS.Domain;
using OVB.Demos.Studies.CQRS.Infrascructure.Data;
using OVB.Demos.Studies.CQRS.Infrascructure.Data.Repositories;
using OVB.Demos.Studies.CQRS.RabbitMQ;
using ProtoBuf;

namespace OVB.Demos.Studies.CQRS.Worker;

public class Worker : BackgroundService
{
    private readonly AccountRepository _accountRepository = new AccountRepository(new DataConnection(new MySqlConnection("Server=localhost;Database=cqrs;Uid=root;Pwd=Lu45139786__;")));
    private RabbitMQConsumer _rabbitMQConsumer = new RabbitMQConsumer();

    public Worker()
    {

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var message = _rabbitMQConsumer.Consume();
            var readonlyMemory = new ReadOnlyMemory<byte>(message);
            var account = Serializer.Deserialize<Account>(readonlyMemory);

            await _accountRepository.CreateAsync(account);

            await Task.Delay(1000, stoppingToken);
        }
    }
}