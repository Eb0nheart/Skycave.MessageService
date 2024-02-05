namespace Skycave.MessageService.DTOs;

public record Message(Guid Id, Guid CreatorId, string CreatorName, DateTime Created, string Content);