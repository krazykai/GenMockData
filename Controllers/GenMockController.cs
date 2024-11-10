using GenMockData.Model.GenMock;
using GenMockData.Service.GenMock;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GenMockData.Controllers
{
    [ApiController]
    [Route("api/GenMock")]
    public class GenMockController : ControllerBase
    {
        private readonly IGenMockDataService _genMockService;

        public GenMockController(IGenMockDataService genMockDataService)
        {
            _genMockService = genMockDataService;
        }

        /// <summary>
        /// Generate mock data string
        /// </summary>
        /// <param name="genMockRequest"></param>
        /// <returns> string </returns>
        [HttpPost]
        [Route("DataString")]
        [SwaggerOperation(Summary = "Generate Mock Data", Description = "Generate Mock Data")]
        public async Task<ActionResult<string>> GenMockDataString([FromBody] GenMockRequest genMockRequest)
        {
            try
            {
                if (genMockRequest.responseFormat.ToLower() == "sql")
                {
                    if (string.IsNullOrEmpty(genMockRequest.tableName) == true)
                    {
                        return BadRequest("tableName is required.");
                    }
                }

                string mockDataString = await _genMockService.GenMock(genMockRequest);

                return mockDataString;
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("DataFile")]
        [SwaggerOperation(Summary = "Generate Mock Data File in Specified Format", Description = "Generate Mock Data File in Specified Format")]
        public async Task<ActionResult<IFormFile>> GenMockDataFile([FromBody] GenMockRequest genMockRequest)
        {
            try
            {
                if (genMockRequest.responseFormat.ToLower() == "sql")
                {
                    if (string.IsNullOrEmpty(genMockRequest.tableName) == true)
                    {
                        return BadRequest("TableName is required.");
                    }
                }

                string mockDataString = await _genMockService.GenMock(genMockRequest);

                ActionResult mockDataFile = await _genMockService.ParseMockDataStringToFile(mockDataString, genMockRequest.responseFormat);

                return mockDataFile;
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
