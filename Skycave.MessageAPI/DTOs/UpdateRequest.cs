namespace Skycave.MessageService.DTOs;

public record UpdateRequest(Guid MessageId, Guid RequestingUserId, string UpdatedMessage);
