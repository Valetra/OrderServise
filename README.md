# Servise for ordering and processing orders.
## This servise have 3 projects that constitutes a full service for creating an order/preorder by customer in establishment and processing it in admin panel.
### Service projects: RESTful API(done), ChatBot(done), AdminPanel(done).


## Two files need to be added for successful  execution :
### 1) OrderServise/ChatBot/private_data.cs
```
namespace private_data
{
    public class MyPrivateData
    {
        public string TelegramBotToken = "<Telegram bot token>";
    }
}
```
### 2) OrderServise/RESTful API/appsettings.json
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
