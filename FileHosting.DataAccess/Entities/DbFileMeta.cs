using FileHosting.Domain.Attributes;

namespace FileHosting.Domain.Entities;

public class DbFileMeta
{
    public Guid Id { get; set; }
    public long Size { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public Guid FileDataId { get; set; }
}