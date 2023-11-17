// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

//builder.Services.AddHostedService<TypeOfBackgroundService>();

using var host = builder.Build();

host.Run();