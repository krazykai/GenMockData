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

        public OpenAIController(IOpenAIService openAIService)
        {
            _openAIService = openAIService;
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
                string jsonResponse = await _openAIService.Chat(content);
                return Ok(jsonResponse);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
