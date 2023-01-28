# TelegramChatGPT

Es un chatbot que utiliza la API de OPENAI, para la generar una conversacion.
Tambien puede generar imagenes.

```csharp
using ChatGPT;
//Keys.json es una archivo que contiene las dos APIKeys
ChatTelegram chatTelegram = new ChatTelegram("Keys.json");
await chatTelegram.ChatearAsync();
```

```json
{
    "TokenTelegram":"APIKeyTelegram",
    "TokenOPENAI": "APIKeyOPENAI"
}
```
#### o tambien.

```csharp
using ChatGPT;
ChatTelegram chatTelegram = new ChatTelegram("TokenChatGPT", "TokenTelegram");
await chatTelegram.ChatearAsync();
```

### Documentacion
- https://beta.openai.com/
- https://core.telegram.org/
- https://telegrambots.github.io/book/
- https://beta.openai.com/docs/api-reference/completions