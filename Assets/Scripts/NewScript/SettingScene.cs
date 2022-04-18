using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingScene : MonoBehaviourPunCallbacks
{
    [SerializeField]
    public int maxPlayers = 10;
    public int VirusNumber = 2;
    public int myMaxPlayers // ตัวแปรสำหรับเก็บค่าจำนวนผู้เล่นสูงสุด
    {
        get //getter
        {
            return maxPlayers; // คืนค่าจำนวนผู้เล่นสูงสุด
        }
        set //setter
        {
            value = value % 11; //ไม่ให้จำนวนผู้เล่นสูงสุดเกิน 10
            value = value <= 4 ? 5 : value;  //ไม่ให้จำนวนผู้เล่นสูงสุดต่ำกว่า 5
            maxPlayers = value;  //กำหนดค่าผู้เล่นสูงสุด
        }
    }

    public int myMaxVirus // ตัวแปรสำหรับเก็บค่าจำนวนผู้เล่นไวรัส
    {
        get // getter
        {
            return VirusNumber;  // คืนค่าจำนวนผู้เล่นไวรัส
        }
        set
        {
            value = value % 3; //ไม่ให้จำนวนผู้เล่นไวรัสมากกว่า 2
            value = value <= 0 ? 1 : value; //ไม่ให้จำนวนผู้เล่นไวรัสต่ำกว่า 1
            VirusNumber = value; //กำหนดค่าจำนวนผู้เล่นไวรัส
        }
    }
    public Text roomtxt; //ตัวแปรเก็บค่า Text แสดงผลจำวนผู้เล่นสูงสุด
    public Text Virustxt; //ตัวแปรเก็บค่า Text แสดงผลจำนวนผู้เล่นไวรัส
    string st = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; // ตัวอักษร A-Z
    public int randomCharacter = 5; //จำนวนตัวอักษรของชื่อห้องที่ต้องการสุ่ม

    public void OnCreateButtonPressed()
    {
        RoomOptions roomOptions = new RoomOptions(); //สร้าง object RoomOptions
        string[] lobbyOptions = new string[1]; //สร้าง list of string
        lobbyOptions[0] = VirusNumber.ToString(); //เก็บค่า จำนวนผู้เล่นไวรัส
        roomOptions.MaxPlayers = (byte)myMaxPlayers; //เก็บค่า จำนวนผู้เล่นสูงสุด
        roomOptions.BroadcastPropsChangeToAll = true; // ตั้งค่าห้องให้ประกาศการเปลี่ยนแปลงค่าห้อง
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "VirusNumber", VirusNumber } }; // ตั้งค่าห้อง
        roomOptions.CustomRoomPropertiesForLobby = lobbyOptions; // ตั้งค่าห้อง
        PhotonNetwork.CreateRoom(getRandomRoomName(), roomOptions, null); // สุ่มชื่อห้อง ตั้งค่าห้อง และ สร้างห้อง
    }

    public void OnIncreasePlayerPressed()
    {
        myMaxPlayers += 1; //เพิ่มจำนวนผู้เล่นสูงสุด 1 คน
    }

    public void OnDecreasePlayerPressed()
    {
        myMaxPlayers -= 1; //ลดจำนวนผู้เล่นสูงสุด 1 คน

    }
    public void OnIncreaseVirusPressed()
    {
        myMaxVirus += 1; //เพิ่มจำนวนผู้เล่นไวรัส 1 คน
    }

    public void OnDecreaseVirusPressed()
    {
        myMaxVirus -= 1; //ลดจำนวนผู้เล่นไวรัส 1 คน
    }
    private void Update()
    {
        roomtxt.text = myMaxPlayers.ToString(); //แสดงผลจำนวนผู้เล่นสูงสุด
        Virustxt.text = myMaxVirus.ToString(); //แสดงผลจำนวนผู้เล่นไวรัส
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene("Lobby1"); //เปลี่ยนไปยัง Scene Lobby1
    }

    private string getRandomRoomName() // สุ่ม A-Z 5 ตัว
    {
        // random character
        string _roomname = "";
        for (int i = 0; i < randomCharacter; i++)
        {
            _roomname += st[UnityEngine.Random.Range(0, st.Length)];
        }
        return _roomname;
    }
    public void OnBackBtnPressed()
    {
        SceneManager.LoadScene("main_menu_scene"); //ออกจาก lobby
    }
    // public override void OnLeftLobby()
    // {
    //     SceneManager.LoadScene("main_menu_scene"); //กลับไปยัง MainMenu
    // }
}
