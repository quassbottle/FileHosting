namespace FileHosting.DataAccess.Attributes;

public class FieldAttribute : Attribute
{
    public string Title { get; }
    
    public FieldAttribute(string title)
    {
        Title = title;
    }
}