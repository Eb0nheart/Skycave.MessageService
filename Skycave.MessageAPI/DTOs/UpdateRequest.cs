namespace Skycave.MessageService.DTOs;

public record UpdateRequest(Guid Id, Guid UserId, string UpdatedMessage);
