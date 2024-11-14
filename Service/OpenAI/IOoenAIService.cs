namespace GenMockData.Service.OpenAI
{
    public interface IOpenAIService
    {
        Task<string> Chat(string content);
    }
}
