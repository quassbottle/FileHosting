namespace FileHosting.Domain.Exceptions;

public class FileNotFoundOnServerException : Exception
{
    protected FileNotFoundOnServerException(string message)
        : base(message)
    {
    }
}