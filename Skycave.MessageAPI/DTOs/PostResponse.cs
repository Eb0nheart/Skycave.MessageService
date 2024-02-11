namespace Skycave.MessageService.DTOs;

public record PostResponse(Guid MessageId, string CreatorName, string Message);