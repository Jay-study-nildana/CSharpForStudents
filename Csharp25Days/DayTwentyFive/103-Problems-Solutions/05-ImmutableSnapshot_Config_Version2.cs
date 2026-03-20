// 05-ImmutableSnapshot_Config.cs
// Immutable configuration snapshot with atomic publish for readers.

using System;
using System.Threading;

public record Config(string ConnectionString, int MaxItems);

public class ConfigProvider
{
    // Volatile or 'ref' publish to ensure memory visibility
    private volatile Config _current = new Config("default", 100);

    public Config Get() => _current; // readers read snapshot without locks

    public void Update(Config newConfig)
    {
        if (newConfig == null) throw new ArgumentNullException(nameof(newConfig));
        // atomic publish: assign new immutable instance
        _current = newConfig;
        // readers will see either old or new instance, no locks needed
    }
}

/*
Notes:
- Immutable record ensures readers don't mutate shared data.
- Volatile assignment ensures publish ordering/visibility.
- Good for read-heavy, occasional write scenarios (copy-on-write).
*/