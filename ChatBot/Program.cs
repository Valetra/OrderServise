using private_data;

MyPrivateData myData = new MyPrivateData();

CancellationTokenSource cancellationTokenSource = new();

RadBot radBot = new(myData.TelegramBotToken, cancellationTokenSource.Token);

Console.WriteLine($"Start listening for @{await radBot.GetUsername()}");

Console.ReadLine();

cancellationTokenSource.Cancel();
