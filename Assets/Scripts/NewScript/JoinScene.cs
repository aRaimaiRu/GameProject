using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class JoinScene : MonoBehaviour
{
    public InputField roomNameInput;
    public void OnJoinedPressed()
    {
        PhotonNetwork.JoinRoom(roomNameInput.text);
    }
}
