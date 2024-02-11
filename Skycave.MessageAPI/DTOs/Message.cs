using System.ComponentModel.DataAnnotations;

namespace Skycave.MessageService.DTOs;

/// <param name="Id"></param>
/// <param name="CreatorId"></param>
/// <param name="CreatorName"></param>
/// <param name="Created">DateTime of creation of the message</param>
/// <param name="Content">The message posted on the wall</param>
public record Message(Guid Id, Guid CreatorId, string CreatorName, DateTime Created, string Content);