using System;
using System.Collections;
using System.Collections.Concurrent;

namespace PrepareMail;

public class ConcurrentHashSet<T> : IReadOnlyCollection<T>
    where T : notnull
{
    const byte DummyByte = byte.MinValue;
    private readonly ConcurrentDictionary<T, byte> _concurrentDictionary = new();

    public void Add(T item) => _concurrentDictionary.TryAdd(item, DummyByte);

    public void Remove(T item) => _concurrentDictionary.TryRemove(item, out _);
    public void Clear() => _concurrentDictionary.Clear();

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return _concurrentDictionary.Keys.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _concurrentDictionary.GetEnumerator();
    }

    public int Count => _concurrentDictionary.Keys.Count;
}