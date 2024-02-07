using Microsoft.AspNetCore.Mvc;
using Skycave.MessageService.DTOs;
using Skycave.MessageService.Storage;
using System.ComponentModel.DataAnnotations;

namespace Skycave.MessageService.Controllers;

[Route("[controller]")]
public class WallController(ILogger<WallController> logger, MessageStorage storage) : ControllerBase
{
    /// <summary>
    /// Gets messages from the wall in the given room.
    /// </summary>
    /// <param name="roomId">Which room the wall to retrieve messages from is in.</param>
    /// <param name="page">Required parameter for standard pagination operation.</param>
    /// <param name="pageSize">Optional for pagination as default is 20 messages.</param>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Message>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> GetMessagesAsync([Required] Guid roomId, [Required] int page, int? pageSize = 20)
    {
        if (page < 0)
        {
            return TypedResults.BadRequest("Page cannot be less than 0!");
        }

        try
        {
            var messages = await storage.GetPostsOnWallAsync(roomId, page, pageSize.Value);
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

    /// <summary>
    /// Posts a message on the wall in the specified room.
    /// </summary>
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
            var wallMessage = await storage.AddPostToWallAsync(dto.RoomId, creator, dto.Message);

            var response = new PostResponse(wallMessage.Id, creator.Name, wallMessage.Message);
            return TypedResults.Created(default(string), response);
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Couldn't save message for request {Request}", dto);
            return TypedResults.Problem();
        }
    }

    /// <summary>
    /// Edits a message on the wall.
    /// </summary>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> UpdateMessage([FromBody] UpdateRequest dto)
    {
        try
        {
            await storage.UpdatePostOnWallAsync(dto.MessageId, dto.UpdatedMessage);
            return TypedResults.Ok();
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Couldn't update message for request {Request}", dto);
            return TypedResults.Problem();
        }
    }
}
