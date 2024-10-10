namespace GenMockData.Service.OpenAI
{
    public interface IOpenAIService
    {
        Task<string> Chat(string content, HttpClient httpClient, string openaiApiKey);
    }
}
