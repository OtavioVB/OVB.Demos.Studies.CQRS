using Microsoft.AspNetCore.Mvc;
using OVB.Demos.Studies.CQRS.Domain;
using OVB.Demos.Studies.CQRS.Infrascructure.Data.Repositories;
using OVB.Demos.Studies.CQRS.RabbitMQ;
using ProtoBuf;

namespace OVB.Demos.Studies.CQRS.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class SendController : ControllerBase
{
    private RabbitMQPublisher publisher = new RabbitMQPublisher();

    [HttpPost]
    public async Task<IActionResult> Post(
        [FromServices] AccountRepository accountRepository)
    {
        var account = new Account(Guid.NewGuid(), "CQRS Account Name");

        await accountRepository.CreateAsync(account);

        using var memory = new MemoryStream();
        Serializer.Serialize(memory, account);
        publisher.Public(memory.ToArray());

        return StatusCode(201);
    }
}