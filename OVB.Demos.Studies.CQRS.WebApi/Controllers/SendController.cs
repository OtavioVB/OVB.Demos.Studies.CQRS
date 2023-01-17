using Microsoft.AspNetCore.Mvc;
using OVB.Demos.Studies.CQRS.Domain;
using OVB.Demos.Studies.CQRS.Infrascructure.Data.Repositories;
using OVB.Demos.Studies.CQRS.RabbitMQ;
using ProtoBuf;
using System.Data.SQLite;

namespace OVB.Demos.Studies.CQRS.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class SendController : ControllerBase
{
    private RabbitMQPublisher publisher = new RabbitMQPublisher();

    [HttpPost]
    public async Task<IActionResult> Post()
    {
        SQLiteConnection connection = new SQLiteConnection("Data source=cqrs.db");
        connection.Open();

        var command = connection.CreateCommand();

        var account = new Account()
        {
            Identifier = Guid.NewGuid().ToString(),
            Name = "Teste"
        };

        command.CommandText = "INSERT INTO Accounts (Identifier, Name) VALUES (@Identifier, @Name)";
        command.Parameters.AddWithValue("@Identifier", account.Identifier);
        command.Parameters.AddWithValue("@Name", account.Name);
        await command.ExecuteNonQueryAsync();

        try
        {
            using var memory = new MemoryStream();
            Serializer.Serialize(memory, account);
            publisher.Public(memory.ToArray());
        }
        catch
        {
            throw;
        }

        return StatusCode(201);
    }
}