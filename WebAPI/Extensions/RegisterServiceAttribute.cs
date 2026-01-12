namespace WebAPI.Extensions;

// Clase que se usa para identificarla como un endpoint
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class RegisterServiceAttribute : Attribute
{
    public ServiceLifetime Lifetime { get; }

    public RegisterServiceAttribute(ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        Lifetime = lifetime;
    }
}
