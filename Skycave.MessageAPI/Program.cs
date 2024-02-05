using Skycave.MessageService.Storage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<MessageStorage, FakeMessageStorage>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

await app.RunAsync();
