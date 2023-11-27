// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
/// 1. read file
/// 2. parse file
/// 3. verify parsed structure
/// 4. parse to petri net
/// 5. parse to gui or not gui
/// 6. invoke gui or not gui
/// 7. wait for result and time it.




//builder.Services.AddHostedService<TypeOfBackgroundService>();

using var host = builder.Build();

host.Run();