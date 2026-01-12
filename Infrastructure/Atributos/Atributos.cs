namespace Infrastructure.Atributos;

internal class Atributos
{
    [AttributeUsage(AttributeTargets.Property)]
    public class Unique : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public class CurrentDateTime : Attribute { }
}
