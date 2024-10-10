using Microsoft.AspNetCore.Identity;

namespace GenMockData.Model.OpenAI
{
    public class OpenAIMessage
    {
        public string role {  get; set; }
        public string content { get; set; }
    }
}
