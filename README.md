After completing program development, test data needs to be generated for testing. This tool is used to generate test data, allowing users to avoid creating test data manually.

Request Body :
{
  "dataCount": 0,
  "genMockColumnList": [
    {
      "columnName": "string",
      "columnType": "string"
    }
  ],
  "responseFormat": "string",
  "tableName": "string"
}
1. int dataCount (Required)
2. List<GenMockColumn> genMockColumnList (Required)
3. string responseFormat (Required)
4. string tableName (option)

API :
There are two APIs for generating test data
1. POST /api/GenMock/DataString
   Returns test data in string format.
3. POST /api/GenMock/DataFile
   Returns test data in file format.
