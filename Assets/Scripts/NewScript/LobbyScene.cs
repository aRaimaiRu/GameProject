using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class LobbyScene : MonoBehaviourPunCallbacks
{
    private List<MyPlayerUIItem> _playerList = new List<MyPlayerUIItem>();
    [SerializeField] private MyPlayerUIItem _playerItemUIPrefab;
    [SerializeField] private Transform _playerListParent;
    public List<Color> myColorpool;
    private List<int> ReserveColorIndex = new List<int>();
    public Text roomName;

    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("Start");
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        // Debug.Log(PhotonNetwork.CurrentRoom.PropertiesListedInLobby);
        // Debug.Log(PhotonNetwork.CurrentRoom.MaxPlayers);
        // Debug.Log(PhotonNetwork.CurrentRoom.CustomProperties["VirusNumber"]);
        // myColorpool.
        // pickcolor
        UpdateReserveColorIndex();
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("ColorIndex"))
        {
            PickedColorIndex(PhotonNetwork.LocalPlayer, (int)PhotonNetwork.LocalPlayer.CustomProperties["ColorIndex"]);
        }
        else
        {
            PickedColorIndex(PhotonNetwork.LocalPlayer, 0);

        }
        UpdatePlayerList();

    }

    // Update is called once per frame
    void Update()
    {

    }




    // }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        // PickedColorIndex(Player player, int _colorIndex);
        // UpdatePlayerList();
    }
    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        ReserveColorIndex.Remove((int)newPlayer.CustomProperties["ColorIndex"]);
        UpdatePlayerList();
    }
    private void UpdatePlayerList()
    {
        //Clear the current list of rooms
        for (int i = 0; i < _playerList.Count; i++)
        {
            Destroy(_playerList[i].gameObject);
        }
        _playerList.Clear();
        ReserveColorIndex.Clear();
        // check if current in room

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }
        //Genereate a new list with the updated info
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            MyPlayerUIItem newPlayerItem = Instantiate(_playerItemUIPrefab);
            newPlayerItem.transform.SetParent(_playerListParent);
            newPlayerItem.SetName(player.Value.NickName);
            // set color
            if (player.Value.CustomProperties.ContainsKey("ColorIndex"))
            {
                newPlayerItem.SetColor(myColorpool[(int)player.Value.CustomProperties["ColorIndex"]]);
            }
            _playerList.Add(newPlayerItem);
        }

    }

    private void PickedColorIndex(Player player, int _colorIndex)
    {
        while (ReserveColorIndex.Contains(_colorIndex))
        {
            _colorIndex++;
        }
        Hashtable hash = new Hashtable();
        hash.Add("ColorIndex", _colorIndex);
        player.SetCustomProperties(hash);
        ReserveColorIndex.Add(_colorIndex);

    }

    private void UpdateReserveColorIndex()
    {
        ReserveColorIndex.Clear();
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if (player.Value.CustomProperties.ContainsKey("ColorIndex"))
            {
                ReserveColorIndex.Add((int)player.Value.CustomProperties["ColorIndex"]);

            }
        }

    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        UpdatePlayerList();
    }
}
