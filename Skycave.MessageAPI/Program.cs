using Microsoft.AspNetCore.Mvc;
using Skycave.MessageService;
using Skycave.MessageService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<MessageStorage, FakeMessageStorage>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/page", async ([FromBody] GetMessages dto, [FromServices] MessageStorage storage) => await storage.GetWallMessagesAsync(dto.Page, dto.PageSize))
    .WithName("GetWallMessages")
    .WithOpenApi();

app.MapPost("", async ([FromBody] CreateMessage dto, [FromServices] MessageStorage storage) => await storage.AddWallMessageAsync(dto.Creator, dto.Message))
    .WithName("PostWallMessages")
    .WithOpenApi();

app.MapPut("", async ([FromBody] UpdateMessage dto, [FromServices] MessageStorage storage) => await storage.UpdateWallMessageAsync(dto.Id, dto.UpdatedMessage))
    .WithName("PutWallMessages")
    .WithOpenApi();

await app.RunAsync();
