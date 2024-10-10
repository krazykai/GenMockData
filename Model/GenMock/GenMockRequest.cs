namespace GenMockData.Model.GenMock
{
    public class GenMockRequest
    {
        // Required
        public int dataCount { get; set; }
        public List<GenMockColumn> genMockColumnList { get; set; }
        public string responseFormat { get; set; }

        // options
        public string? tableName { get; set; }
    }
}
