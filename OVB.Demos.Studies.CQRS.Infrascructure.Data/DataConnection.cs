using System.Data.Common;

namespace OVB.Demos.Studies.CQRS.Infrascructure.Data;

public class DataConnection
{
    private readonly DbConnection _dbConnection;

    public DataConnection(DbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task OpenConnection()
    {
        await _dbConnection.OpenAsync();
    }

    public DbCommand CreateCommand()
    {
        return _dbConnection.CreateCommand();
    }

    public async Task CloseConnection()
    {
        await _dbConnection.CloseAsync();
    }
}
