// See https://aka.ms/new-console-template for more information
// https://github.com/microsoft/reverse-proxy/issues/2143
// サブディレクトリでホストされている gRPC サービスの呼び出し
// https://learn.microsoft.com/ja-jp/aspnet/core/grpc/troubleshoot?view=aspnetcore-8.0#calling-grpc-services-hosted-in-a-sub-directory

using Microsoft.Extensions.DependencyInjection;
using Grpc.Net.Client;
using GrpcClient.Services;
using Microsoft.AspNetCore.Mvc;

Console.WriteLine("サブディレクトリでホストされている gRPC サービスの呼び出し");
Console.WriteLine("https://learn.microsoft.com/ja-jp/aspnet/core/grpc/troubleshoot?view=aspnetcore-8.0#calling-grpc-services-hosted-in-a-sub-directory");

var handler1 = new SubdirectoryHandler(new HttpClientHandler(), "/service1");
var channel1 = GrpcChannel.ForAddress("https://localhost:8000", new GrpcChannelOptions { HttpHandler = handler1 });
var client1 = new GrpcService1.Greeter.GreeterClient(channel1);
var request1 = new GrpcService1.HelloRequest { Name = "Hello World! from service 1" };
var response1 = client1.SayHello(request1);

Console.WriteLine(response1.Message);

var handler2 = new SubdirectoryHandler(new HttpClientHandler(), "/service2");
var channel2 = GrpcChannel.ForAddress("https://localhost:8000", new GrpcChannelOptions { HttpHandler = handler2 });
var client2 = new GrpcService2.Greeter.GreeterClient(channel2);
var request2 = new GrpcService2.HelloRequest { Name = "Hello World! from service 2" };
var response2 = client2.SayHello(request2);
Console.WriteLine(response2.Message);


var serviceCollection = new ServiceCollection();
var handler3 = new SubdirectoryHandler(new HttpClientHandler(), "/service3");

// ここでサービスの追加
serviceCollection.AddScoped<IGreeterService, GreeterService>();
serviceCollection.AddGrpcClient<GrpcService3.Greeter.GreeterClient>(options =>
{
    options.Address = new Uri("https://localhost:8000");
})
    .ConfigureChannel(o =>
{
    o.HttpHandler = handler3;
});

// ServiceProviderを作成
var serviceProvider = serviceCollection.BuildServiceProvider();

// 必要なサービスを取得
var service = serviceProvider.GetRequiredService<IGreeterService>();
var response3 = await service.SayHello("Hello World! from service 3");
Console.WriteLine(response3);

Console.ReadKey();

