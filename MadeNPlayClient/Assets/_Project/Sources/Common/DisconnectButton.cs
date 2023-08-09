using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class DisconnectButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        NetworkManager.Singleton.Shutdown();
        //Debug.LogError("DISCONNECTED");
    }
}