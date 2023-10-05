using FileHosting.DataAccess.Attributes;

namespace FileHosting.DataAccess.Entities;

public class DbFileUrl
{
    [Field("id")]
    public Guid Id { get; set; }
    
    [Field("meta_id")]
    public Guid FileDataId { get; set; }
}