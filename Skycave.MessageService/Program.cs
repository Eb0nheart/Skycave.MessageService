using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Models;
using Skycave.MessageService;
using Skycave.MessageService.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFeatureManagement();
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

var useFakeStorage = builder.Configuration.GetSection($"FeatureManagement:{FeatureFlags.UseFakeStorage}").Get<bool>();
if (useFakeStorage)
{
    builder.Services.AddSingleton<MessageStorage, FakeMessageStorage>();
}
else
{
    builder.Services.AddSingleton<MessageStorage, RedisMessageStorage>();
}

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

await app.RunAsync();
