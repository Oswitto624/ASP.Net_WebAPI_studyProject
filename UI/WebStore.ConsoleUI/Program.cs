using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using WebStore.ConsoleUI;

WebAPITest.Start();

var configuration = new ConfigurationBuilder()
   .AddJsonFile("appsettings.json")
   .Build();

var builder = new HubConnectionBuilder();
var connection = builder
   .WithUrl(configuration["ChatAddress"])
   .Build();

using var registration = connection.On<string>("MessageFromServer", msg => Console.WriteLine("Сообщение от сервера {0}", msg));

Console.Write("Ожидание запуска сервера. Нажмите Enter для продолжения.");
Console.ReadLine();

await connection.StartAsync();
Console.WriteLine("Соединение установлено");

while (true)
{
    var message = Console.ReadLine();
    await connection.SendAsync("SendMessage", message);
}