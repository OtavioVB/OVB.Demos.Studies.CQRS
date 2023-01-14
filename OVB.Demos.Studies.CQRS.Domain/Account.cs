namespace OVB.Demos.Studies.CQRS.Domain;

 public class Account
{
    public Account(Guid identifier, string name)
    {
        Identifier = identifier;
        Name = name;
    }

    public Guid Identifier { get; set; }
    public string Name { get; set; }
}
