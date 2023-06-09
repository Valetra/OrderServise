# RadBot
## Telegram chatbot that allows anyone to create an order in cafe.

## Two files need to be added:
### 1) RadBot/ChatBot/private_data.cs with following code ->
```
namespace private_data
{
    public class MyPrivateData
    {
        public string TelegramBotToken = "<Telegram bot token>";
    }
}
```
### 2) RadBot/RESTful API/appsettings.json with following code ->
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
