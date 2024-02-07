namespace Skycave.MessageService.Storage;

public record Wall(Guid RoomId, List<Post> Posts);

public record Post(Guid Id, DateTime Created, Creator Creator, string Message);

public record Creator(Guid Id, string Name);


public interface MessageStorage
{
    Task<Post> AddPostToWallAsync(Guid roomId, Creator creator, string message);
    
    Task<IEnumerable<Post>> GetPostsOnWallAsync(Guid roomId, int page, int pageSize);

    Task UpdatePostOnWallAsync(Guid id, string message);

}