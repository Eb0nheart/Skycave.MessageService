using Microsoft.OpenApi.Models;
using Skycave.MessageService.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<MessageStorage, FakeMessageStorage>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(configuration =>
{
    configuration.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Skycave - Message Service",
        Version = "v1"
    });

    var filePath = Path.Combine(AppContext.BaseDirectory, "Skycave.MessageService.xml");
    configuration.IncludeXmlComments(filePath);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

await app.RunAsync();
