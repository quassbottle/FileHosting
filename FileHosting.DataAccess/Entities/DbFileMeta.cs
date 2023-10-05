using FileHosting.DataAccess.Attributes;

namespace FileHosting.DataAccess.Entities;

public class DbFileMeta
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("size")]
    public long Size { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
    
    [Column("type")]
    public string Type { get; set; }
}