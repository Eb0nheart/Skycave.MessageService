
namespace Skycave.MessageService.Storage;

public class RedisMessageStorage : MessageStorage
{
    public Task<Post> AddPostToWallAsync(string positionString, Creator creator, string message)
    {
        throw new NotImplementedException();
    }

    public Task<Post?> GetPost(Guid postId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Post>> GetPostsOnWallAsync(string positionString, int page, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task UpdatePostOnWallAsync(Guid id, string message)
    {
        throw new NotImplementedException();
    }
}
