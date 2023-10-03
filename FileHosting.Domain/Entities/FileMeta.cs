namespace FileHosting.Domain.Entities;

public class FileMeta
{
    public Guid? Guid { get; set; }
    public long? Size { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<FileData> FileDataList { get; set; } = new();
}