using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkStateHandler : MonoBehaviour
{
    private void Start()
    {
        NetworkManager.Singleton.OnClientStopped += (e) => ReloadApp();
        NetworkManager.Singleton.OnTransportFailure += ReloadApp;
    }

    private void OnDisable()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientStopped -= (e) => ReloadApp();
            NetworkManager.Singleton.OnTransportFailure += ReloadApp;
        }
    }

    private void ReloadApp()
    {
        SceneManager.LoadScene(CONSTANTS.MENU_SCENE_INDEX);
        NetworkPrefabManager.Instanse.ResetPrefabs();
    }
}