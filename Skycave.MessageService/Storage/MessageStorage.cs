namespace Skycave.MessageService.Storage;

public record Wall(string PositionString, List<Post> Posts);

public record Post(Guid Id, DateTime Created, Creator Creator, string Message);

public record Creator(Guid Id, string Name);


public interface MessageStorage
{
    Task<Post> AddPostToWallAsync(string positionString, Creator creator, string message);
    
    Task<IEnumerable<Post>> GetPostsOnWallAsync(string positionString, int page, int pageSize);

    Task<Post?> GetPost(Guid postId);

    Task UpdatePostOnWallAsync(Guid id, string message);

}