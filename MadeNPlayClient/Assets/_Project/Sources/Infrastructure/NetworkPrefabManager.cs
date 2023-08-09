using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class NetworkPrefabManager : MonoBehaviour
{
    [SerializeField] private NetworkPrefabsList _networkPrefabsList;

    public static NetworkPrefabManager Instanse { get; private set; }

    private void Awake()
    {
        if (Instanse == null)
            Instanse = this;
        else
            Destroy(gameObject);
    }

    public List<GameObject> GetCurrentPrefabs()
    {
        return NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs.Select(x => x.Prefab).ToList();
    }

    public void ResetPrefabs()
    {
        var currentPrefabs = GetCurrentPrefabs();
        foreach (var prefab in currentPrefabs)
        {
            NetworkManager.Singleton.RemoveNetworkPrefab(prefab);
        }
        RegisterNetworkPrefabs();
    }

    public void RegisterNetworkPrefabs()
    {
        var prefabs = _networkPrefabsList.PrefabList.Select(x => x.Prefab);
        foreach (var prefab in prefabs)
        {
            NetworkManager.Singleton.AddNetworkPrefab(prefab);
        }
    }
}