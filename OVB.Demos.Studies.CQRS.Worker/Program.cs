using MySql.Data.MySqlClient;
using OVB.Demos.Studies.CQRS.Infrascructure.Data;
using OVB.Demos.Studies.CQRS.Infrascructure.Data.Repositories;

namespace OVB.Demos.Studies.CQRS.Worker;

public class Program
{
    public static void Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddHostedService<Worker>();
            })
            .Build();

        host.Run();
    }
}