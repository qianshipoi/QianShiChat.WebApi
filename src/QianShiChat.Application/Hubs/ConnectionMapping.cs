﻿namespace QianShiChat.Application.Hubs;

internal class ConnectionMapping<T> where T : notnull
{

    private readonly Dictionary<T, HashSet<string>> _connections =
        new Dictionary<T, HashSet<string>>();

    public int Count => _connections.Count;

    public bool ContainsKey(T key) => _connections.ContainsKey(key);

    public IEnumerable<string> GetConnections(T key)
    {
        HashSet<string>? connections;
        if (_connections.TryGetValue(key, out connections))
        {
            return connections;
        }
        return Enumerable.Empty<string>();
    }

    public void Add(T key, string connectionId)
    {
        lock (_connections)
        {
            HashSet<string>? connections;
            if (!_connections.TryGetValue(key, out connections))
            {
                connections = new HashSet<string>();
                _connections.Add(key, connections);
            }

            lock (connections)
            {
                connections.Add(connectionId);
            }
        }
    }

    public void Remove(T key, string connectionId)
    {
        lock (_connections)
        {
            HashSet<string>? connections;
            if (!_connections.TryGetValue(key, out connections))
            {
                return;
            }

            lock (connections)
            {
                connections.Remove(connectionId);
                if (connections.Count == 0)
                {
                    _connections.Remove(key);
                }
            }
        }
    }
}
