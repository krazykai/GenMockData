using GenMockData.Helper;
using GenMockData.Service.GenMock;
using GenMockData.Service.OpenAI;

var builder = WebApplication.CreateBuilder(args);


/* 讀取環境變數 */
// 獲取當前工作目錄中的 .env 文件路徑
var env = Path.Combine(Directory.GetCurrentDirectory(), ".env");
// 檢查 .env 文件是否存在
if (File.Exists(env))
{
    // 讀取.env 文件中的所有行
    var envVariables = File.ReadAllLines(env);
    // 逐行處理，每行應包含一個環境變量的定義（如 KEY=VALUE）
    foreach (var envVariable in envVariables)
    {
        // 將每行拆分為鍵和值
        var keyValue = envVariable.Split('=');
        // 如果拆分後有兩個部分（鍵和值），則設置環境變量
        if (keyValue.Length == 2)
        {
            Environment.SetEnvironmentVariable(keyValue[0], keyValue[1]);
        }
    }
}
// 獲取設置的 OPENAI_API_KEY 環境變量
var openaiApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");


// Register IHttpClientFactory
builder.Services.AddHttpClient();

// Add services to the container.
builder.Services.AddScoped<IOpenAIService, OpenAIService>();
builder.Services.AddScoped<IGenMockDataService, GenMockService>();

// 將 OpenAIAPIHelper 的註冊方式使用 AddHttpClient，以便正確管理 HttpClient 的生命週期。
// 使用 AddHttpClient 可以重複用同一個 HttpClient 實例，避免頻繁建立新的 HttpClient 實例，而浪費系統資源，提升高頻率 HTTP 請求時的效能。
builder.Services.AddHttpClient<OpenAIAPIHelper>();



// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations(); // 啟用 Swagger 註解功能
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
