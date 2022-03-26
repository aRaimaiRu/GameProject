using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JoinScene : MonoBehaviourPunCallbacks
{
    public InputField roomNameInput;  //ตัวแปร InputField รับค่าจาก InputField
    public void OnJoinedPressed()
    {
        PhotonNetwork.JoinRoom(roomNameInput.text); //เข้าร่วมห้องตาม ค่าจาก InputField
    }
    public void OnBackBtnPressed()
    {
        SceneManager.LoadScene("main_menu_scene"); // กลับไปยังหน้า MainMenu
    }
    public override void OnLeftLobby()
    {
        SceneManager.LoadScene("main_menu_scene"); // กลับไปยังหน้า MainMenu

    }
}
