namespace Skycave.MessageService.DTOs;

public record PostResponse(Guid MessageId, string Creator, string Message);