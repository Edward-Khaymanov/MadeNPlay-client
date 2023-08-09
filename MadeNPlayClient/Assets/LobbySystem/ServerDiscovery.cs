using System;
using System.Net;

namespace LobbySystem
{
    public class ServerDiscovery
    {
        private readonly string _secretKey;
        private readonly UDPMessenger _server = new UDPMessenger();
        private readonly UDPMessenger _client = new UDPMessenger();

        public ServerDiscovery(string secretKey)
        {
            _secretKey = secretKey;
        }

        public void FindServers(Action<NetworkLobbyData, IPEndPoint> callback)
        {
            var targetIP = new IPEndPoint(IPAddress.Broadcast, CONSTANTS.LOBBY_SERVER_PORT);
            _client.Send(_secretKey, targetIP);
            _client.Listen(CONSTANTS.LOBBY_CLIENT_PORT, callback);
        }

        public void StartServer(Func<NetworkLobbyData> responseRequest)
        {
            _server.Listen<string>(CONSTANTS.LOBBY_SERVER_PORT, (key, endPoint) =>
            {
                if (KeyValid(key))
                {
                    var endpoint = new IPEndPoint(endPoint.Address, CONSTANTS.LOBBY_CLIENT_PORT);
                    _server.Send(responseRequest(), endpoint);
                }
            });
        }

        public void StopServer()
        {
            _server.StopListen();
        }

        public void StopFindServers()
        {
            _client.StopListen();
        }

        private bool KeyValid(string key)
        {
            return key == _secretKey;
        }
    }
}