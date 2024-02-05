namespace Skycave.MessageService.Storage;

public record Creator(Guid Id, string Name);

public record WallMessage(Guid Id, DateTime Created, Creator Creator, string Message);

public interface MessageStorage
{
    Task<WallMessage> AddWallMessageAsync(Creator creator, string message);

    Task UpdateWallMessageAsync(Guid id, string message);

    Task<IEnumerable<WallMessage>> GetWallMessagesAsync(int page, int pageSize);
}