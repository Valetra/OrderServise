using private_data;
using ChatBot.Managers;

MyPrivateData myData = new MyPrivateData();

CancellationTokenSource cancellationTokenSource = new();

string apiPath = "https://localhost:7096/"; //http://localhost:5132/

ControllerManager controllerManager = new(apiPath);

RadBot radBot = new(myData.TelegramBotToken, cancellationTokenSource.Token, controllerManager);

Console.WriteLine($"Start listening for @{await radBot.GetUsername()}");

Console.ReadLine();

cancellationTokenSource.Cancel();
