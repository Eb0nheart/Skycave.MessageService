namespace Skycave.MessageService.DTOs;

public record PostRequest(Guid RoomId, Guid CreatorId, string CreatorName, string? Message);
