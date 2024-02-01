using System.Collections.Concurrent;

namespace Skycave.MessageService.Data;

public class FakeMessageStorage(ILogger<FakeMessageStorage> logger) : MessageStorage
{
    // TODO: anden collection type
    private readonly ConcurrentQueue<WallMessage> messages = [];

    public Task<WallMessage> AddWallMessageAsync(string creator, string messageContent)
    {
        var id = Guid.NewGuid();
        var created = DateTime.Now;
        var message = new WallMessage(id, created, creator, messageContent);
        messages.Enqueue(message);
        return Task.FromResult(message);
    }

    public Task<IEnumerable<WallMessage>> GetWallMessagesAsync(int page, int pageSize)
    {
        var pageStart = page * pageSize;
        var pageEnd = pageStart + pageSize;
        var range = new Range(new Index(pageStart), new Index(pageEnd));
        var messageOnPage = messages
            .OrderByDescending(message => message.Created)
            .Take(range);
        return Task.FromResult(messageOnPage);
    }

    public Task UpdateWallMessageAsync(Guid id, string message)
    {
        return Task.CompletedTask;
    }
}