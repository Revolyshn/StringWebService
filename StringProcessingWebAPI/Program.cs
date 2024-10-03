using StringProcessingWebAPI.Handlers;

var builder = WebApplication.CreateBuilder(args);

// ������ ������������ �� ����� appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// ����������� �������� ��� ��������� ������������
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IStringProcessHandler, StringProcessHandler>();
builder.Services.AddTransient<IRandomCharacterRemover, RandomCharacterRemover>();

var app = builder.Build();

// ������������ HTTP-���������.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
