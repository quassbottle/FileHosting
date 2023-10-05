using FileHosting.DataAccess.Attributes;

namespace FileHosting.DataAccess.Entities;

public class DbFileUrl
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("meta_id")]
    public Guid FileDataId { get; set; }
}