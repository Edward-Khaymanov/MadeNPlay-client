using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace LobbySystem
{
    public class UDPMessenger
    {
        private UdpClient _listenClient;
        private CancellationTokenSource _listenTokenSource = new CancellationTokenSource();

        public void Send<T>(T data, IPEndPoint targetEndpoint)
        {
            SendInternal(data, targetEndpoint);
        }

        public void Listen<T>(ushort port, Action<T, IPEndPoint> callback)
        {
            ListenInternal(port, _listenTokenSource.Token, callback);
        }

        public void StopListen()
        {
            _listenTokenSource.Cancel();
            _listenTokenSource = new CancellationTokenSource();
            _listenClient?.Close();
        }

        private async void SendInternal<T>(T data, IPEndPoint targetEndpoint)
        {
            try
            {
                using (var sendClient = new UdpClient() { EnableBroadcast = true, MulticastLoopback = false })
                {
                    var encodedData = new BinarySerializer().Serialize(data);
                    await sendClient.SendAsync(encodedData, encodedData.Length, targetEndpoint);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        private async void ListenInternal<T>(ushort port, CancellationToken cancellationToken, Action<T, IPEndPoint> callback)
        {
            try
            {
                using (_listenClient = new UdpClient(port) { EnableBroadcast = true, MulticastLoopback = false })
                {
                    var binarySerializer = new BinarySerializer();
                    while (cancellationToken.IsCancellationRequested == false)
                    {
                        var result = await _listenClient.ReceiveAsync();
                        if (result.Buffer.Length != 0)
                        {
                            var data = binarySerializer.Deserialize<T>(result.Buffer);
                            if (EqualityComparer<T>.Default.Equals(data, default) == false)
                                callback?.Invoke(data, result.RemoteEndPoint);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }
    }
}