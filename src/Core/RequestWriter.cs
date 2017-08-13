using System;
using Newtonsoft.Json;

public interface IRequestWriter
{
    void Write(Request req);
}

[Local, Test]
public class LocalRequestWriter : IRequestWriter
{
    public int WriteCount { get; private set; }
    public void Write(Request req)
    {
        WriteCount += 1;
        Console.WriteLine($"LOCAL: {JsonConvert.SerializeObject(req)}");
    }
}

[Lambda]
public class LambdaRequestWriter : IRequestWriter
{
    public void Write(Request req)
    {
        Console.WriteLine($"LAMBDA: {JsonConvert.SerializeObject(req)}");
    }
}