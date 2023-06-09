# RadBot
## This solution have 3 projects that constitutes a full service for creating by customer an order/preorder in cafe and processing it in admin panel.
### Service projects: RESTful API(done), ChatBot(in progress), AdminPanel(not started).


## Two files need to be added for successful  execution :
### 1) RadBot/ChatBot/private_data.cs
```
namespace private_data
{
    public class MyPrivateData
    {
        public string TelegramBotToken = "<Telegram bot token>";
    }
}
```
### 2) RadBot/RESTful API/appsettings.json
```
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=<domain>;Port=<port>;Database=<DB name>;User Id=<user id>;Password=<user password>"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```
