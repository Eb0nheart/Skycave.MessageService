namespace Skycave.MessageService.DTOs;

public record PostRequest(Guid CreatorId, string CreatorName, string? Message);
