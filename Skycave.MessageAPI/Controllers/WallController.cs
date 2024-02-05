using Microsoft.AspNetCore.Mvc;
using Skycave.MessageService.DTOs;
using Skycave.MessageService.Storage;

namespace Skycave.MessageService.Controllers;

[Route("[controller]")]
public class WallController(ILogger<WallController> logger, MessageStorage storage) : ControllerBase
{
    [HttpGet]
    [Route("page={page:int}&pageSize={pageSize:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type= typeof(IEnumerable<Message>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> GetMessagesAsync(int page, int pageSize)
    {
        if (page < 0 || pageSize < 1)
        {
            return TypedResults.BadRequest("Page cannot be less than 0 and pagesize cannot be less than 1!");
        }

        try
        {
            var messages = await storage.GetWallMessagesAsync(page, pageSize);
            if (!messages.Any())
            {
                return TypedResults.NoContent();
            }

            return TypedResults.Ok(messages.Select(message 
                => new Message(message.Id, message.Creator.Id, message.Creator.Name, message.Created, message.Message)));
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Can't get messages from storage!");
            return TypedResults.Problem();
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PostResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> PostMessage([FromBody] PostRequest dto)
    {
        if(string.IsNullOrWhiteSpace(dto.CreatorName) || string.IsNullOrWhiteSpace(dto.Message))
        {
            return TypedResults.BadRequest("Creator");
        }

        try
        {
            var creator = new Creator(dto.CreatorId, dto.CreatorName);
            var wallMessage = await storage.AddWallMessageAsync(creator, dto.Message);

            var response = new PostResponse(creator.Id, creator.Name, wallMessage.Message);
            return TypedResults.Created(default(string), response);
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Couldn't save message for request {Request}", dto);
            return TypedResults.Problem();
        }
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> UpdateMessage([FromBody] UpdateRequest dto)
    {
        try
        {
            await storage.UpdateWallMessageAsync(dto.Id, dto.UpdatedMessage);
            return TypedResults.Ok();
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Couldn't update message for request {Request}", dto);
            return TypedResults.Problem();
        }
    }
}
