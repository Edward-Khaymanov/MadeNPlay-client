using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public struct TeamInfo : INetworkSerializable, IEquatable<TeamInfo>
{
    private int _id;
    private FixedString32Bytes _name;
    private int _maxPlayers;

    public TeamInfo(int id, string name, int slotCount)
    {
        _id = id;
        _name = name.Substring(0, Mathf.Min(name.Length, 32));
        _maxPlayers = slotCount;
    }

    public int Id => _id;
    public FixedString32Bytes Name => _name;
    public int MaxPlayers => _maxPlayers;

    public bool Equals(TeamInfo other)
    {
        return 
            _id == other._id && 
            _maxPlayers == other._maxPlayers && 
            _name.ToString() == other._name.ToString();
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        if (serializer.IsReader)
        {
            var reader = serializer.GetFastBufferReader();
            reader.ReadValueSafe(out _id);
            reader.ReadValueSafe(out _name);
            reader.ReadValueSafe(out _maxPlayers);
        }
        else
        {
            var writer = serializer.GetFastBufferWriter();
            writer.WriteValueSafe(_id);
            writer.WriteValueSafe(_name);
            writer.WriteValueSafe(_maxPlayers);
        }
    }
}