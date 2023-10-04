namespace FileHosting.DataAccess.Entities;

public class DbFileMeta
{
    public Guid Id { get; set; }
    public long Size { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
}