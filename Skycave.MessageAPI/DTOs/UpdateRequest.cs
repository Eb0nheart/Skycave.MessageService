using System.ComponentModel.DataAnnotations;

namespace Skycave.MessageService.DTOs;

public class UpdateRequest
{
    [Required]
    public Guid MessageId { get; init; }

    /// <summary>
    /// Id of the the user wanting to update a message
    /// </summary>
    [Required]
    public Guid RequestingUserId { get; init; }

    [Required]
    public string UpdatedMessage { get; init; }
}

