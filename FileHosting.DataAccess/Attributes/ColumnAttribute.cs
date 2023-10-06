namespace FileHosting.DataAccess.Attributes;

public class ColumnAttribute : Attribute
{
    public string Title { get; }
    
    public ColumnAttribute(string title)
    {
        Title = title;
    }
}