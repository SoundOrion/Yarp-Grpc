namespace GrpcClient.Services;
public class GreeterService: IGreeterService
{
    private readonly GrpcService3.Greeter.GreeterClient _client;

    public GreeterService(GrpcService3.Greeter.GreeterClient client)
    {
        _client = client;
    }

    public async Task<string> SayHello(string message)
    {
        var reply = await _client.SayHelloAsync(new GrpcService3.HelloRequest { Name = message });
        return reply.Message;
    }
}
