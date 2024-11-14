using GenMockData.Helper;
using GenMockData.Model.OpenAI;

namespace GenMockData.Service.OpenAI
{
    public class OpenAIService : IOpenAIService
    {
        private readonly OpenAIAPIHelper _openAIAPIHelper;

        public OpenAIService(OpenAIAPIHelper openAIAPIHelper)
        {
            _openAIAPIHelper = openAIAPIHelper;
        }

        /// <summary>
        /// Chat with OpenAI
        /// </summary>
        /// <param name="content"></param>
        /// <returns> string </returns>
        public async Task<string> Chat(string content)
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

            string? jsonResponse = await _openAIAPIHelper.ChatWithOpenAI(openAIMessages);

            return jsonResponse;
        }
    }
}
