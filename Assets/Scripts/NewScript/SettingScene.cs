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
            Debug.Log(value);
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
            Debug.Log(value);
            value = value % 3;
            value = value <= 0 ? 1 : value;
            VirusNumber = value;
        }
    }
    public Text roomtxt;
    public Text Virustxt;

    public void OnCreateButtonPressed()
    {
        RoomOptions roomOptions = new RoomOptions();
        string[] lobbyOptions = new string[1];
        lobbyOptions[0] = VirusNumber.ToString();
        roomOptions.MaxPlayers = (byte)myMaxPlayers;
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "VirusNumber", VirusNumber } };
        roomOptions.CustomRoomPropertiesForLobby = lobbyOptions;
        PhotonNetwork.CreateRoom("roombame", roomOptions, null);
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
    // public override void OnCreatedRoom()
    // {
    //     Debug.Log("Createroom");
    //     PhotonNetwork.JoinRoom("roombame");

    // }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        // Debug.Log(PhotonNetwork.CurrentRoom.PropertiesListedInLobby);
        Debug.Log(PhotonNetwork.CurrentRoom.MaxPlayers);
        Debug.Log(PhotonNetwork.CurrentRoom.CustomProperties["VirusNumber"]);
        SceneManager.LoadScene("Lobby1");


    }




}
