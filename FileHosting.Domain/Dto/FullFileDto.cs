namespace FileHosting.Domain.Dto;

public class FullFileDto
{
    public string Name { get; set; }
    public long SizeInBytes { get; set; }
    public byte[] Content { get; set; }
    public string Type { get; set; }
}