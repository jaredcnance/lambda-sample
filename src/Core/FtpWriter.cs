using System;
using System.IO;
using System.Threading.Tasks;

public interface IFtpWriter
{
    Task WriteAsync(Stream stream, string name);
}

[Lambda]
public class FtpWriter : IFtpWriter
{
    public Task WriteAsync(Stream stream, string name)
    {
        throw new NotImplementedException();
    }
}

[Local, Test]
public class FileSystemWriter : IFtpWriter
{
    public Task WriteAsync(Stream stream, string name)
    {
        throw new NotImplementedException();
    }
}