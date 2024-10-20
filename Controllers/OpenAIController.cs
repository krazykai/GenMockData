using GenMockData.Service.OpenAI;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GenMockData.Controllers
{
    [ApiController]
    [Route("api/OpenAI")]
    public class OpenAIController : ControllerBase
    {
        private readonly IOpenAIService _openAIService;
        private readonly HttpClient _httpClient;
        private readonly string _openaiApiKey;

        public OpenAIController(IOpenAIService openAIService, IHttpClientFactory httpClientFactory)
        {
            _openAIService = openAIService;

            _httpClient = httpClientFactory.CreateClient();

            _openaiApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        }

        /// <summary>
        /// Chat with OpenAI API
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Chat with OpenAI API", Description = "Chat with OpenAI API")]
        public async Task<IActionResult> Chat([FromBody] string content)
        {
            try
            {
                var jsonResponse = await _openAIService.Chat(content, _httpClient, _openaiApiKey);
                return Ok(jsonResponse);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
