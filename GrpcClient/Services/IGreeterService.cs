namespace GrpcClient.Services;

public interface IGreeterService
{
    Task<string> SayHello(string message);
}