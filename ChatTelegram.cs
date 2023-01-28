using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatGPT
{
    public class ChatTelegram
    {

        private string TokenTelegram{get;set;}
        private string TokenChatGPT {get;set;}

        public ChatTelegram(string TokenChatGPT, string TokenTelegram){
            this.TokenChatGPT = TokenChatGPT;
            this.TokenTelegram = TokenTelegram;

        }

        public ChatTelegram(string FilePathToken){
            string content = System.IO.File.ReadAllText(FilePathToken);
            Token token = JsonSerializer.Deserialize<Token>(content);
            this.TokenChatGPT = token.TokenOPENAI;
            this.TokenTelegram = token.TokenTelegram;
        }
        public async Task ChatearAsync()
        {
            var botClient = new TelegramBotClient(TokenTelegram);
            using CancellationTokenSource cts = new();

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
            };

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Empezando a escuchar @{me.Username}");
            Console.ReadLine();

            // Send cancellation request to stop bot
            cts.Cancel();

            async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            {
                // Only process Message updates: https://core.telegram.org/bots/api#message
                if (update.Message is not { } message)
                    return;
                // Only process text messages
                if (message.Text is not { } messageText)
                    return;

                var chatId = message.Chat.Id;

                Console.Write($"\nReceived a '{messageText}' message in chat {chatId}.\n");
                ChatGPT.ChatGPTClass chatGPTClass = new ChatGPT.ChatGPTClass(TokenChatGPT);
                if (messageText.Contains("/image "))
                {
                    messageText = messageText.Replace("/image ","");
                    Images cp = chatGPTClass.GetChatImage(messageText, 1);
                    Console.WriteLine(cp.data.Equals(null));
                    Console.WriteLine($"Sending: \"{cp.data[0].url}\"");
                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: cp.data[0].url,
                        cancellationToken: cancellationToken);
                }
                else
                {
                    Completion cp = chatGPTClass.GetChatText(messageText);
                    Console.WriteLine($"Sending: \"{cp.choices[0].text}\"");
                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: cp.choices[0].text,
                        cancellationToken: cancellationToken);
                }
            }

            Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
            {
                var ErrorMessage = exception switch
                {
                    ApiRequestException apiRequestException
                        => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                    _ => exception.ToString()
                };

                Console.WriteLine(ErrorMessage);
                return Task.CompletedTask;
            }
        }
    }
}