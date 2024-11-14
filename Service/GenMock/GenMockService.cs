using GenMockData.Model.GenMock;
using GenMockData.Service.OpenAI;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using System;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json.Nodes;

namespace GenMockData.Service.GenMock
{
    public class GenMockService : IGenMockDataService
    {
        private readonly IOpenAIService _openAIService;

        public GenMockService(IOpenAIService openAIService)
        {
            _openAIService = openAIService;
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
            string tableName = "";

            if (genMockRequest.responseFormat.ToLower() == "sql")
            {
                format = "SQL insert statements";
            }
            if (!string.IsNullOrEmpty(genMockRequest.tableName))
            {
                tableName = " for the table " + genMockRequest.tableName;
            }

            string content = $" Please generate {dataCount} rows of mock data {tableName} with the following fields: {genMockColumnString}. The response format should be {format} as a single string without formatting or line breaks. Only provide the mock data, without any additional dialogue or commentary.";

            var response = await _openAIService.Chat(content);

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
            else if (fileType.ToLower() == "sql")
            {
                // Convert the mock data string to a byte array using UTF-8 encoding
                byte[] byteArray = Encoding.UTF8.GetBytes(mockDataString);

                // Create a memory stream from the byte array
                MemoryStream stream = new MemoryStream(byteArray);

                // Create a FileStreamResult from the memory stream, setting the content type as SQL and specifying the download file name
                fileStreamResult = new FileStreamResult(stream, "application/sql")
                {
                    FileDownloadName = "mockData.sql"
                };
            }

            return fileStreamResult;
        }
    }
}
