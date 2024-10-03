using StringProcessingWebAPI.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Чтение конфигурации из файла appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Регистрация сервисов для внедрения зависимостей
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IStringProcessHandler, StringProcessHandler>();
builder.Services.AddTransient<IRandomCharacterRemover, RandomCharacterRemover>();

var app = builder.Build();

// Конфигурация HTTP-пайплайна.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
