using GenMockData.Model.GenMock;
using GenMockData.Service.OpenAI;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json.Nodes;

namespace GenMockData.Service.GenMock
{
    public class GenMockService : IGenMockDataService
    {
        private readonly IOpenAIService _openAIService;
        private readonly HttpClient _httpClient;
        private readonly string _openaiApiKey;

        public GenMockService(IOpenAIService openAIService, IHttpClientFactory httpClientFactory)
        {
            _openAIService = openAIService;

            _httpClient = httpClientFactory.CreateClient();

            _openaiApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        }


        /// <summary>
        /// Generate mock data
        /// </summary>
        /// <param name="genMockRequest"></param>
        /// <returns> string </returns>
        public async Task<string> GenMock(GenMockRequest genMockRequest)
        {
            int dataCount = genMockRequest.dataCount;
            string genMockColumnString = await GetGenMockColumnString(genMockRequest.genMockColumnList);
            string format = genMockRequest.responseFormat;

            if (genMockRequest.responseFormat.ToLower() == "sql")
            {
                format = "SQL 插入語句";
            }

            string content = $" 請產生 {dataCount} 筆測試資料，欄位有 {genMockColumnString}，回傳的格式是 {format}。請直接回覆測試資料即可，不用加上其他對話內容。";

            var response = await _openAIService.Chat(content, _httpClient, _openaiApiKey);

            if (response == null)
            {
                return "";
            }
            var responseJsonObject = JsonObject.Parse(response);

            string resultContent = responseJsonObject["choices"][0]["message"]["content"].ToString();

            resultContent = resultContent.Replace("\n", "").Replace("\r", "");

            return resultContent;
        }

        private async Task<string> GetGenMockColumnString(List<GenMockColumn> genMockColumnList)
        {
            string genMockColumnString = "";

            for (int i = 0; i < genMockColumnList.Count; i++)
            {
                if (i != 0)
                {
                    genMockColumnString += ", ";
                }
                genMockColumnString += $"{i + 1}. {genMockColumnList[i].columnName} ({genMockColumnList[i].columnType})";
            }

            return genMockColumnString;
        }

        public async Task<ActionResult> ParseMockDataStringToFile(string mockDataString, string fileType)
        {
            FileStreamResult fileStreamResult = null;


            if (fileType.ToLower() == "json")
            {
                byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(mockDataString);
                MemoryStream stream = new MemoryStream(byteArray);

                fileStreamResult = new FileStreamResult(stream, "application/json")
                {
                    FileDownloadName = "mockData.json"
                };
            }


            return fileStreamResult;
        }
    }
}
