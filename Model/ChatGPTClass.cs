using System.Net.Http.Headers;
using System.Text.Json;

namespace ChatGPT
{
    public class ChatGPTClass
    {
        public enum models
        {
            babbage,
            ada,
            davinci,
            text_davinci_001,
            curie_instruct_beta,
            text_davinci_003,
            code_cushman_001,
            code_davinci_002,
            text_ada_001,
            text_davinci_002,
            text_curie_001,
            davinci_instruct_beta,
            text_babbage_001,
            curie
        }

        public enum Resolution
        {
            x256,
            x512,
            x1024
        }

        private string Token { get; set; }

        public ChatGPTClass()
        {

        }
        public ChatGPTClass(string token)
        {
            this.Token = token;
        }

        public Completion GetChatText(string prompt = "Que dia es hoy?", int max_tokens = 2048, double temperature = 0.9, models modelo = models.text_davinci_003)
        {

            string urlBase = "https://api.openai.com/v1/completions";
            string dataSend = $"{{\"model\": \"{modelo.ToString().Replace('_', '-')}\", " +
                            $"\"prompt\": \"{prompt}\", " +
                            $"\"max_tokens\": {max_tokens}, " +
                            $"\"temperature\": {temperature}}}";
            string responseJSON = BaseResponse(urlBase, dataSend);
            return JsonSerializer.Deserialize<Completion>(responseJSON);
        }

        public Images GetChatImage(string prompt, int numeroImg, Resolution resolution = Resolution.x512)
        {
            string urlBase = "https://api.openai.com/v1/images/generations";
            string dataSend = $"{{\"prompt\":\"{prompt}\", \"n\":{numeroImg}, \"size\":\"{ResolutionStr(resolution)}\"}}";

            string responseJSON = BaseResponse(urlBase, dataSend);
            Console.WriteLine(responseJSON);

            Images img = JsonSerializer.Deserialize<Images>(responseJSON);

            try{
                img.data.Equals(null);
                return img;
                 
            }catch(Exception ex){
                Console.WriteLine("Segunda pasada");
                Images imgExection = new Images();
                var dat = new Data();
                dat.url = "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4d/OpenAI_Logo.svg/640px-OpenAI_Logo.svg.png";
                imgExection.data = new List<Data>();
                imgExection.data.Add(dat);
                imgExection.created = 0;
                return imgExection;
            }
        }

        private string BaseResponse(string urlBase, string stringContent)
        {
            string data = "";
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), urlBase))
                {
                    request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {this.Token}");
                    request.Content = new StringContent(stringContent);
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = httpClient.SendAsync(request).Result;
                    data = new StreamReader(response.Content.ReadAsStream()).ReadToEnd();
                }
            }
            return data;
        }


        private string ResolutionStr(Resolution res)
        {
            switch (res)
            {
                case Resolution.x256:
                    return "256x256";
                case Resolution.x512:
                    return "512x512";
                case Resolution.x1024:
                    return "1024x1024";
                default:
                    return "512x512";
            }
        }
    }
}