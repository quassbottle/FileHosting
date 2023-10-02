namespace FileHosting.Models;

public class SseEvent
{
    public string Name { get; set; }
    public object Data { get; set; }
    public string Id { get; set; } = "";
    public int? Retry { get; set; }

    public SseEvent(string name, object data)
    {
        Name = name;
        Data = data;
    }
}