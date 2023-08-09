using System;

[Serializable]
public class NetworkLobbyData
{
    public NetworkLobbyData(
        Guid lobbyId, 
        string serverName, 
        ushort connectionPort,
        int activePlayers,
        int maxPlayers, 
        NetworkGameData networkGameData 
        )
    {
        LobbyId = lobbyId;
        ServerName = serverName;
        ConnectionPort = connectionPort;
        ActivePlayers = activePlayers;
        MaxPlayers = maxPlayers;
        NetworkGameData = networkGameData;
    }

    public Guid LobbyId { get; set; }
    public string ServerName { get; set; }
    public ushort ConnectionPort { get; set; }
    public int ActivePlayers { get; set; }
    public int MaxPlayers { get; set; }
    public NetworkGameData NetworkGameData { get; set; }
}