using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JoinScene : MonoBehaviourPunCallbacks
{
    public InputField roomNameInput;
    public void OnJoinedPressed()
    {
        PhotonNetwork.JoinRoom(roomNameInput.text);
    }
    public void OnBackBtnPressed()
    {
        SceneManager.LoadScene("main_menu_scene");
    }


    public override void OnLeftLobby()
    {
        SceneManager.LoadScene("main_menu_scene");

    }
}
