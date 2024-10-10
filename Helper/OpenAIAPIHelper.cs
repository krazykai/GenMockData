using GenMockData.Model.OpenAI;
using System.Text.Json;

namespace GenMockData.Helper
{
    public class OpenAIAPIHelper
    {
        private readonly HttpClient _httpClient;
        private readonly string _openaiApiKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenAIAPIHelper"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client used to send requests.</param>
        /// <param name="openaiApiKey">The OpenAI API key for authentication.</param>
        public OpenAIAPIHelper(HttpClient httpClient, string openaiApiKey)
        {
            _httpClient = httpClient;
            _openaiApiKey = openaiApiKey;
        }

        /// <summary>
        /// Sends a request to the OpenAI API and gets the response.
        /// </summary>
        /// <param name="message">The list of messages to send to the model.</param>
        /// <returns></returns>
        public async Task<string> ChatWithOpenAI(List<OpenAIMessage> message)
        {
            // Create the request payload
            var openAIRequest = new
            {
                //model = "gpt-3.5-turbo",
                model = "gpt-4o",
                messages = message
            };
            StringContent content = new StringContent(JsonSerializer.Serialize(openAIRequest), System.Text.Encoding.UTF8, "application/json");

            // Create the HTTP request
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://api.openai.com/v1/chat/completions"),
                Headers =
                {
                    { "Authorization", $"Bearer {_openaiApiKey}" }
                },
                Content = content
            };

            // Send the request and get the response
            HttpResponseMessage response = await _httpClient.SendAsync(httpRequestMessage);

            // Ensure the request was successful
            if (response.IsSuccessStatusCode)
            {
                // Read and return the response content as a string
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                // throw exception
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}. Error: {errorContent}");
            }
        }
    }
}
