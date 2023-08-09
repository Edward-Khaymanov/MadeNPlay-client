using MadeNPlayShared;
using System;
using Unity.Netcode;

[Serializable]
public struct UserTeam : INetworkSerializable, IEquatable<UserTeam>
{
    private NetworkUser _user;
    private int _teamId;
    private bool _isReady;

    public UserTeam(NetworkUser user, int teamId, bool isReady)
    {
        _user = user;
        _teamId = teamId;
        _isReady = isReady;
    }

    public NetworkUser User => _user;
    public int TeamId => _teamId;
    public bool IsReady => _isReady;

    public bool Equals(UserTeam other)
    {
        return 
            other.User.Equals(_user) && 
            _teamId == other._teamId && 
            _isReady == other._isReady;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        if (serializer.IsReader)
        {
            serializer.SerializeValue(ref _user);

            var reader = serializer.GetFastBufferReader();
            reader.ReadValueSafe(out _teamId);
            reader.ReadValueSafe(out _isReady);
        }
        else
        {
            serializer.SerializeValue(ref _user);

            var writer = serializer.GetFastBufferWriter();
            writer.WriteValueSafe(_teamId);
            writer.WriteValueSafe(_isReady);
        }
    }
}