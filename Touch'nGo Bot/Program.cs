using Newtonsoft.Json;
using System.Net.Http.Headers;
using Touch_nGo_Bot.Model;

namespace Touch_nGo_Bot
{
    internal class Program
    {
        private static string cookie = "COOKIE_HERE";
        private static string csrfToken = "CSRF_TOKEN_HERE";

        private static string itemId = "3175099305";
        private static string skuId = "16072707014";
        private static int quantity = 1;

        static async Task Main(string[] args)
        {
            //pandai2 le buat looping kalau nak monitor
            var result = await CheckItemStatus();
            Console.WriteLine(result);
            Console.ReadLine();
        }

        //taktau public api untuk tengok product info. so guna add to cart api; atc success = item in stock
        private static async Task<string> CheckItemStatus()
        {
            var handler = new HttpClientHandler();
            string contents;

            using (var httpClient = new HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://cart.lazada.com.my/cart/api/add"))
                {
                    request.Headers.TryAddWithoutValidation("authority", "cart.lazada.com.my");
                    request.Headers.TryAddWithoutValidation("accept", "application/json, text/plain, */*");
                    request.Headers.TryAddWithoutValidation("accept-language", "en-US,en;q=0.9,en-GB;q=0.8");
                    request.Headers.TryAddWithoutValidation("cookie", cookie);
                    request.Headers.TryAddWithoutValidation("origin", "https://www.lazada.com.my");
                    request.Headers.TryAddWithoutValidation("referer", "https://www.lazada.com.my/");
                    request.Headers.TryAddWithoutValidation("sec-ch-ua", "\"Not?A_Brand\";v=\"8\", \"Chromium\";v=\"108\", \"Google Chrome\";v=\"108\"");
                    request.Headers.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
                    request.Headers.TryAddWithoutValidation("sec-ch-ua-platform", "\"Windows\"");
                    request.Headers.TryAddWithoutValidation("sec-fetch-dest", "empty");
                    request.Headers.TryAddWithoutValidation("sec-fetch-mode", "cors");
                    request.Headers.TryAddWithoutValidation("sec-fetch-site", "same-site");
                    request.Headers.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36");
                    request.Headers.TryAddWithoutValidation("x-csrf-token", csrfToken);
                    request.Headers.TryAddWithoutValidation("x-requested-with", "XMLHttpRequest");

                    request.Content = new StringContent("[{\"itemId\":\"" + itemId + "\",\"skuId\":\"" + skuId + "\",\"quantity\":" + quantity +  "}]");
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;charset=UTF-8");

                    var response = await httpClient.SendAsync(request);
                    contents = await response.Content.ReadAsStringAsync();
                }
            }

            var json = JsonConvert.DeserializeObject<Json.Root>(contents);
            string msgInfo = "";

            if (json == null)
                msgInfo = "Response is null";
            else if (!json.success)
                msgInfo = "Request failed";
            else if (!json.module.success && json.module.msgInfo.Contains("It is out of stock"))
                msgInfo = "Out of stock";
            else
                msgInfo = "Item in stock";

            return msgInfo;
        }
    }
}
