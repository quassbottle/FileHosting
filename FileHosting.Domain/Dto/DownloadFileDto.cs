namespace FileHosting.Domain.Dto;

public class DownloadFileDto
{
    public byte[] Data { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
}