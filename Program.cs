using GenMockData.Service.GenMock;
using GenMockData.Service.OpenAI;

var builder = WebApplication.CreateBuilder(args);


/* Ū�������ܼ� */
// �����e�u�@�ؿ����� .env �����|
var env = Path.Combine(Directory.GetCurrentDirectory(), ".env");
// �ˬd .env ���O�_�s�b
if (File.Exists(env))
{
    // Ū��.env ��󤤪��Ҧ���
    var envVariables = File.ReadAllLines(env);
    // �v��B�z�A�C�����]�t�@�������ܶq���w�q�]�p KEY=VALUE�^
    foreach (var envVariable in envVariables)
    {
        // �N�C��������M��
        var keyValue = envVariable.Split('=');
        // �p�G����ᦳ��ӳ����]��M�ȡ^�A�h�]�m�����ܶq
        if (keyValue.Length == 2)
        {
            Environment.SetEnvironmentVariable(keyValue[0], keyValue[1]);
        }
    }
}
// ����]�m�� OPENAI_API_KEY �����ܶq
var openaiApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");



// Add services to the container.
builder.Services.AddScoped<IOpenAIService, OpenAIService>();
builder.Services.AddScoped<IGenMockDataService, GenMockService>();

// Register IHttpClientFactory
builder.Services.AddHttpClient();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
