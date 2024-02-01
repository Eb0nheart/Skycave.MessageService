namespace Skycave.MessageService.Data;

public record WallMessage(Guid Id, DateTime Created, string Creator, string MessageContent);

public interface MessageStorage
{
    Task<WallMessage> AddWallMessageAsync(string creator, string messageContent);

    Task UpdateWallMessageAsync(Guid id, string message);

    Task<IEnumerable<WallMessage>> GetWallMessagesAsync(int page, int pageSize);
}