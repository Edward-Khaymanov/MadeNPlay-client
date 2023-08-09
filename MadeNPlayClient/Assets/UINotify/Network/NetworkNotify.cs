using Unity.Netcode;
using UnityEngine;

namespace UINotify
{
    public class NetworkNotify : NetworkBehaviour
    {
        public static NetworkNotify Instance { get; private set; }

        public override void OnNetworkSpawn()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public override void OnNetworkDespawn()
        {
            if (Instance == this)
                Instance = null;
        }

        [ClientRpc]
        public void PushClientRPC(
            string message,
            float visibilityDuration,
            AnimationType animation = default,
            NotificationPosition position = default,
            bool destroy = true,
            ClientRpcParams targetClients = default)
        {
            LocalNotify.Show(message, visibilityDuration, animation, position, destroy);
        }

        [ClientRpc]
        public void PushClientRPC(
            string message,
            float visibilityDuration,
            NotificationStyleType type,
            AnimationType animation = default,
            NotificationPosition position = default,
            bool destroy = true,
            ClientRpcParams targetClients = default)
        {
            LocalNotify.Show(message, visibilityDuration, type, animation, position, destroy);
        }

        [ClientRpc]
        public void PushClientRPC(
            string message,
            float visibilityDuration,
            Color backgroundColor,
            Color textColor,
            AnimationType animation = default,
            NotificationPosition position = default,
            bool destroy = true,
            ClientRpcParams targetClients = default)
        {
            var style = new NotificationStyle()
            {
                Background = backgroundColor,
                Text = textColor,
            };

            LocalNotify.Show(message, visibilityDuration, style, animation, position, destroy);
        }
    }
}