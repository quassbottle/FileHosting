using FileHosting.DataAccess.Attributes;

namespace FileHosting.DataAccess.Entities;

public class DbFileNameDataTypeJoin
{
    [Column("name")]
    public string Name { get; set; }
    
    [Column("data")]
    public byte[] Data { get; set; }
    
    [Column("type")]
    public string Type { get; set; }
}