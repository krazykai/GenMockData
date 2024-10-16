using GenMockData.Helper;
using GenMockData.Model.OpenAI;

namespace GenMockData.Service.OpenAI
{
    public class OpenAIService : IOpenAIService
    {
        /// <summary>
        /// Chat with OpenAI
        /// </summary>
        /// <param name="content"></param>
        /// <param name="httpClient"></param>
        /// <param name="openaiApiKey"></param>
        /// <returns> string </returns>
        public async Task<string> Chat(string content, HttpClient httpClient, string openaiApiKey)
        {
            OpenAIMessage openAIMessageSystem = new OpenAIMessage()
            {
                role = "system",
                content = "You are a helpful assistant.",
            };
            OpenAIMessage openAIMessageUser = new OpenAIMessage()
            {
                role = "user",
                content = content,
            };
            List<OpenAIMessage> openAIMessages = new List<OpenAIMessage>();
            openAIMessages.Add(openAIMessageSystem);
            openAIMessages.Add(openAIMessageUser);

            OpenAIAPIHelper openAIAPIHelper = new OpenAIAPIHelper(httpClient, openaiApiKey);
            string? jsonResponse = await openAIAPIHelper.ChatWithOpenAI(openAIMessages);

            return jsonResponse;
        }
    }
}
