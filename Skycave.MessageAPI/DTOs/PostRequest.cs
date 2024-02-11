using System.ComponentModel.DataAnnotations;

namespace Skycave.MessageService.DTOs;

public class PostRequest
{
    /// <summary>
    /// Format is (x,y,z)
    /// </summary>
    [Required]
    public string PositionString { get; init; }

    [Required]
    public Guid CreatorId { get; init; }

    [Required]
    public string CreatorName { get; init; }

    [Required]
    public string Message { get; init; } 
}
