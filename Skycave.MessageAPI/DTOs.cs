namespace Skycave.MessageService;

public record GetMessages(int Page, int PageSize);

public record CreateMessage(string Creator, string Message);

public record UpdateMessage(Guid Id, string UpdatedMessage);