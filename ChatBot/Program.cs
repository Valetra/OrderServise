using private_data;

MyPrivateData myData = new MyPrivateData();

CancellationTokenSource cancellationTokenSource = new();

string apiPath = "http://localhost:5132/";

RadBot radBot = new(myData.TelegramBotToken, cancellationTokenSource.Token, apiPath);

Console.WriteLine($"Start listening for @{await radBot.GetUsername()}");

Console.ReadLine();

cancellationTokenSource.Cancel();
