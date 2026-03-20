// 06-MessageQueue_ChannelPattern.cs
// Producer/consumer with single-consumer Channel to serialize access to a shared index.

using System;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Collections.Concurrent;

public class WorkItem { public string Key; public int Value; }

public class Indexer
{
    private readonly Channel<WorkItem> _channel = Channel.CreateUnbounded<WorkItem>();
    private readonly ConcurrentDictionary<string, int> _index = new();

    public Indexer()
    {
        // start single consumer background worker
        _ = Task.Run(() => ConsumeAsync());
    }

    public ValueTask EnqueueAsync(WorkItem w) => _channel.Writer.WriteAsync(w);

    private async Task ConsumeAsync()
    {
        await foreach (var item in _channel.Reader.ReadAllAsync())
        {
            // single-threaded updates to index logic could be here; using ConcurrentDictionary as example
            _index.AddOrUpdate(item.Key, item.Value, (k, old) => old + item.Value);
        }
    }

    public int Get(string key) => _index.TryGetValue(key, out var v) ? v : 0;
}

/*
Why this is safe:
- All updates are serialized through the channel's single consumer, eliminating races on shared mutable logic.
- Producers are free to run concurrently and push work into the channel.
*/