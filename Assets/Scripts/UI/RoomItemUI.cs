using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItemUI : MonoBehaviour
{
    public LobbyNetworkManager LobbyNetworkParent;
    [SerializeField] private Text _roomName;
    public void SetName(string roomName)
    {
        _roomName.text = roomName;
    }
    public void OnJoinPressed()
    {
        LobbyNetworkParent.JoinRoom(_roomName.text);
    }
}
