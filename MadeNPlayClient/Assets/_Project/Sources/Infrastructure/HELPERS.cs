using Unity.Netcode;

public static class HELPERS
{
    public static ClientRpcParams GetClient(ulong clientId)
    {
        var target = new ClientRpcParams()
        {
            Send = new ClientRpcSendParams()
            {
                TargetClientIds = new ulong[] { clientId }
            }
        };

        return target;
    }

    public static ClientRpcParams GetAllClients()
    {
        var target = new ClientRpcParams()
        {
            Send = new ClientRpcSendParams()
            {
                TargetClientIds = NetworkManager.Singleton.ConnectedClientsIds
            }
        };

        return target;
    }
}