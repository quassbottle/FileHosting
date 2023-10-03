namespace FileHosting.Domain.Attributes;

public class OrdinalAttribute : Attribute
{
    public int Ordinal { get; private set; }
    
    public OrdinalAttribute(int ordinal)
    {
        Ordinal = ordinal;
    }
}