using ProtoBuf;

namespace OVB.Demos.Studies.CQRS.Domain;

[ProtoContract()]
 public class Account
{
    [ProtoMember(1)]
    public string? Identifier { get; set; }
    [ProtoMember(2)]
    public string? Name { get; set; }
}
