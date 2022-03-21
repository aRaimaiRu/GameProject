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
    public int myMaxPlayers
    {
        get
        {
            return maxPlayers;
        }
        set
        {
            value = value % 11;
            value = value <= 4 ? 5 : value;
            maxPlayers = value;
        }
    }

    public int myMaxVirus
    {
        get
        {
            return VirusNumber;
        }
        set
        {
            value = value % 3;
            value = value <= 0 ? 1 : value;
            VirusNumber = value;
        }
    }
    public Text roomtxt;
    public Text Virustxt;
    string st = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public int randomCharacter = 5;

    public void OnCreateButtonPressed()
    {
        RoomOptions roomOptions = new RoomOptions();
        string[] lobbyOptions = new string[1];
        lobbyOptions[0] = VirusNumber.ToString();
        roomOptions.MaxPlayers = (byte)myMaxPlayers;
        roomOptions.BroadcastPropsChangeToAll = true;
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "VirusNumber", VirusNumber } };
        roomOptions.CustomRoomPropertiesForLobby = lobbyOptions;
        PhotonNetwork.CreateRoom(getRandomRoomName(), roomOptions, null);
    }

    public void OnIncreasePlayerPressed()
    {
        myMaxPlayers += 1;

    }

    public void OnDecreasePlayerPressed()
    {
        myMaxPlayers -= 1;

    }
    public void OnIncreaseVirusPressed()
    {
        myMaxVirus += 1;

    }

    public void OnDecreaseVirusPressed()
    {
        myMaxVirus -= 1;

    }
    private void Update()
    {
        roomtxt.text = myMaxPlayers.ToString();
        Virustxt.text = myMaxVirus.ToString();

    }

    public override void OnJoinedRoom()
    {

        SceneManager.LoadScene("Lobby1");
    }

    private string getRandomRoomName()
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
        PhotonNetwork.LeaveLobby();
    }
    public override void OnLeftLobby()
    {
        SceneManager.LoadScene("main_menu_scene");
    }
}
