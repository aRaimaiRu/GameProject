using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyNetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField _roominput;
    [SerializeField] private RoomItemUI _roomItemUIPrefab;
    [SerializeField] private Transform _roomListParent;
    [SerializeField] private Text _statusField;
    [SerializeField] private Button _leaveRoomButton;
    [SerializeField] private Button _startGameButton;
    [SerializeField] private RoomItemUI _playerItemUIPrefab;
    [SerializeField] private Transform _playerListParent;



    private List<RoomItemUI> _roomList = new List<RoomItemUI>();
    private List<RoomItemUI> _playerList = new List<RoomItemUI>();

    void Start()
    {
        Initialize();
        Connect();
    }

    #region PhotonCallbacks
    private void Initialize()
    {
        _leaveRoomButton.interactable = false;
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Master");
        PhotonNetwork.JoinLobby();
    }
    public override void OnConnected()
    {
        Debug.Log("Disconnected");
    }
    public override void OnJoinedLobby()
    {
        _statusField.text = "Joined Lobby";
        Debug.Log("Joined Lobby");
    }
    public override void OnJoinedRoom()
    {
        _statusField.text = "Joined : " + PhotonNetwork.CurrentRoom.Name;
        Debug.Log("Joined Room : " + PhotonNetwork.CurrentRoom.Name);
        _leaveRoomButton.interactable = true;
        if (PhotonNetwork.IsMasterClient)
        {
            _startGameButton.interactable = true;
        }

        UpdatePlayerList();
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate");
        Debug.Log("room Count" + roomList.Count);

        UpdateRoomList(roomList);
    }

    public override void OnLeftRoom()
    {
        _statusField.text = "LOBBY";
        Debug.Log("Left Room  ");
        _leaveRoomButton.interactable = false;
        _startGameButton.interactable = false;

        UpdatePlayerList();
        PhotonNetwork.JoinLobby();


    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }
    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }
    #endregion

    private void Connect()
    {
        PhotonNetwork.NickName = "Player" + Random.Range(0, 5000);
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    private void UpdateRoomList(List<RoomInfo> roomList)
    {
        //Clear the current list of rooms
        for (int i = 0; i < _roomList.Count; i++)
        {
            Destroy(_roomList[i].gameObject);
        }
        _roomList.Clear();
        Debug.Log("after clear roomlist count =" + roomList.Count);
        //Genereate a new list with the updated info
        for (int i = 0; i < roomList.Count; i++)
        {
            Debug.Log("Room player count =" + roomList[i].PlayerCount + " = " + roomList[i].Name);
            if (roomList[i].PlayerCount == 0) { continue; }
            RoomItemUI newRoomItem = Instantiate(_roomItemUIPrefab);
            newRoomItem.LobbyNetworkParent = this;
            newRoomItem.SetName(roomList[i].Name);
            newRoomItem.transform.SetParent(_roomListParent);

            _roomList.Add(newRoomItem);

        }
    }
    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(_roominput.text) == false)
        {
            PhotonNetwork.CreateRoom(_roominput.text, new RoomOptions() { MaxPlayers = 4 }, null);
        }

    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();

    }
    private void UpdatePlayerList()
    {
        //Clear the current list of rooms
        for (int i = 0; i < _playerList.Count; i++)
        {
            Destroy(_playerList[i].gameObject);
        }
        _playerList.Clear();
        // check if current in room

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }
        //Genereate a new list with the updated info
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            RoomItemUI newPlayerItem = Instantiate(_playerItemUIPrefab);
            newPlayerItem.transform.SetParent(_playerListParent);
            newPlayerItem.SetName(player.Value.NickName);
            _playerList.Add(newPlayerItem);
        }

    }

    public void OnStartGamePressed()
    {
        PhotonNetwork.LoadLevel("game_scene");
    }





}
