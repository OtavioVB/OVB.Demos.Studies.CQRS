using OVB.Demos.Studies.CQRS.Domain;
using System.Data.Common;

namespace OVB.Demos.Studies.CQRS.Infrascructure.Data.Repositories;

public class AccountRepository
{
    private readonly DataConnection _dataConnection;

    public AccountRepository(DataConnection dataConnection)
    {
        _dataConnection = dataConnection;
    }

    public async Task CreateAsync(Account account)
    {
        using (var command = _dataConnection.CreateCommand())
        {
            command.CommandText = $"INSERT INTO Accounts (Identifier, Name) VALUES ('{account.Identifier}', '{account.Name}')";
            await command.ExecuteNonQueryAsync();
        }
    }
}
