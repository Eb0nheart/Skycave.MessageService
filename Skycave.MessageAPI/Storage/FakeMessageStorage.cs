namespace Skycave.MessageService.Storage;

public class FakeMessageStorage(ILogger<FakeMessageStorage> logger) : MessageStorage
{
    private readonly static object wallLock = new();
    private readonly List<Wall> walls = [];

    public Task<Post> AddPostToWallAsync(Guid roomId, Creator creator, string messageContent)
    {
        var id = Guid.NewGuid();
        var created = DateTime.Now;
        var post = new Post(id, created, creator, messageContent);

        lock(wallLock)
        {
            var wall = walls.SingleOrDefault(wall => wall.RoomId == roomId);
            if(wall is null)
            {
                var newWall = new Wall(roomId, [post]);
                walls.Add(newWall);
            }
            else
            {
                wall.Posts.Add(post);
            }
        }

        return Task.FromResult(post);
    }

    public Task<IEnumerable<Post>> GetPostsOnWallAsync(Guid roomId, int page, int pageSize)
    {
        var pageStart = page * pageSize;
        var pageEnd = pageStart + pageSize;
        var range = new Range(new Index(pageStart), new Index(pageEnd));

        lock (wallLock)
        {
            var wall = walls.SingleOrDefault(wall => wall.RoomId == roomId);
            if(wall is null)
            {
                return Task.FromResult(Enumerable.Empty<Post>());
            }

            var postsOnWall = wall.Posts.Take(range);
            return Task.FromResult(postsOnWall);
        }
    }

    public Task UpdatePostOnWallAsync(Guid postId, string message)
    {
        lock (wallLock)
        {
            // TODO: Error handling. Hvad hvis wall ikke findes? Hvad hvis post ikke findes? 
            var wall = walls.Find(wall => wall.Posts.Exists(post => post.Id == postId));
            var post = wall.Posts.Find(post => post.Id == postId);
            var updatedPost = post with { Message = message };
            var postIndex = wall.Posts.IndexOf(post);
            wall.Posts[postIndex] = updatedPost;
        }

        return Task.CompletedTask;
    }
}