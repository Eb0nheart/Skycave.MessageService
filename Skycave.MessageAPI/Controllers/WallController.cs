using Microsoft.AspNetCore.Mvc;
using Skycave.MessageService.DTOs;
using Skycave.MessageService.Storage;
using System.ComponentModel.DataAnnotations;

namespace Skycave.MessageService.Controllers;

[Route("[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public class WallController(ILogger<WallController> logger, MessageStorage storage) : ControllerBase
{
    /// <summary>
    /// Gets messages from the wall in the given room.
    /// </summary>
    /// <response code="200">Returns the requested messages</response>
    /// <response code="204">Request was successful but no messages found</response>
    /// <response code="400">Invalid arguments provided</response>
    /// <response code="500">Internal server error, f.ex. database connection issues</response>
    /// <param name="positionString">Which room the wall to retrieve messages from is in. format is (x,y,z)</param>
    /// <param name="pageNumber">Required parameter for standard pagination operation.</param>
    /// <param name="pageSize">Optional for pagination as default is 20 messages.</param>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Message>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> GetMessagesAsync([Required] string positionString, [Required] int pageNumber, int? pageSize = 20)
    {
        if (pageNumber < 0)
        {
            return TypedResults.BadRequest("Page cannot be less than 0!");
        }

        try
        {
            var messages = await storage.GetPostsOnWallAsync(positionString, pageNumber, pageSize.Value);
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
    /// <response code="201">Resource was created successfully</response>
    /// <response code="400">Invalid arguments provided</response>
    /// <response code="500">Internal server error, f.ex. db issues</response>
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
            var wallMessage = await storage.AddPostToWallAsync(dto.PositionString, creator, dto.Message);

            var response = new PostResponse(wallMessage.Id, creator.Name, wallMessage.Message);
            return TypedResults.Created(default(string), response);
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Couldn't save message for request {@Request}", dto);
            return TypedResults.Problem();
        }
    }

    /// <summary>
    /// Edits a message on the wall.
    /// </summary>
    /// <response code="200">Message was updated successfully</response>
    /// <response code="401">User tried to update another user's message</response>
    /// <response code="404">Requested message to update doesn't exist</response>
    /// <response code="500">Internal server error, f.ex. db issues</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> UpdateMessage([FromBody] UpdateRequest dto)
    {
        try
        {
            await storage.UpdatePostOnWallAsync(dto.MessageId, dto.UpdatedMessage);
            return TypedResults.Ok();
        }
        catch(ArgumentException ex)
        {
            logger.LogCritical(ex, "Invalid update request {@UpdateRequest}", dto);
            return TypedResults.NotFound();
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Couldn't update message for request {@Request}", dto);
            return TypedResults.Problem();
        }
    }
}
