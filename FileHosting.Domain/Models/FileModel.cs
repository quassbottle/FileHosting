namespace FileHosting.Domain.Models;

public class FileModel
{
    public string Name { get; set; }
    public long SizeInBytes { get; set; }
    public byte[] Data { get; set; }
    public string Type { get; set; }
}