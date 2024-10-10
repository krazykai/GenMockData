using GenMockData.Model.GenMock;
using Microsoft.AspNetCore.Mvc;

namespace GenMockData.Service.GenMock
{
    public interface IGenMockDataService
    {
        Task<string> GenMock(GenMockRequest genMockRequest);
        Task<ActionResult> ParseMockDataStringToFile(string mockDataString, string fileType);
    }
}
