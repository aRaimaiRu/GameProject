using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class LobbyScene : MonoBehaviourPunCallbacks
{
    private List<MyPlayerUIItem> _playerList = new List<MyPlayerUIItem>();
    [SerializeField] private MyPlayerUIItem _playerItemUIPrefab;
    [SerializeField] private Transform _playerListParent;
    public List<Color> myColorpool;
    public List<int> ReserveColorIndex = new List<int>();
    public Text roomName;
    private enum pickmode
    {
        Increase,
        Decrease
    }
    public Button PlayBtn;

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
            PickedColorIndex(PhotonNetwork.LocalPlayer, (int)PhotonNetwork.LocalPlayer.CustomProperties["ColorIndex"], pickmode.Increase);
        }
        else
        {
            PickedColorIndex(PhotonNetwork.LocalPlayer, 0, pickmode.Increase);

        }
        UpdatePlayerList();

    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 1)
        {
            PlayBtn.gameObject.SetActive(true);
        }
        else
        {
            PlayBtn.gameObject.SetActive(false);

        }

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
            if (player.Value == PhotonNetwork.LocalPlayer)
            {
                newPlayerItem.LocalChange(this);
            }
            // set color
            if (player.Value.CustomProperties.ContainsKey("ColorIndex"))
            {
                newPlayerItem.SetColor(myColorpool[(int)player.Value.CustomProperties["ColorIndex"]]);
            }
            _playerList.Add(newPlayerItem);
        }

    }

    private void PickedColorIndex(Player player, int _colorIndex, pickmode _pickmode)
    {

        for (int i = 0; i < ReserveColorIndex.Count; i++)
        {
            Debug.Log(ReserveColorIndex[i]);
        }
        Debug.Log(_colorIndex + "ReserveColorIndex Contain" + ReserveColorIndex.Contains(_colorIndex));
        while (ReserveColorIndex.Contains(_colorIndex))
        {
            Debug.Log("In while" + _colorIndex + " :" + ReserveColorIndex.Contains(_colorIndex));
            if (_pickmode == pickmode.Increase)
            {
                _colorIndex = (_colorIndex + 1) % myColorpool.Count;

            }
            else if (_pickmode == pickmode.Decrease)
            {
                _colorIndex = (_colorIndex - 1) % myColorpool.Count;
                if (_colorIndex < 0)
                {
                    _colorIndex = myColorpool.Count - 1;
                }
            }
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
        UpdateReserveColorIndex();
        UpdatePlayerList();
    }
    public void OnIncreaaseIndexPressed(Player _player)
    {
        int newind = ((int)_player.CustomProperties["ColorIndex"]) + 1;
        newind = newind % myColorpool.Count;
        PickedColorIndex(_player, newind, pickmode.Increase);
    }
    public void OnDecreaseIndexPressed(Player _player)
    {
        int newind = ((int)_player.CustomProperties["ColorIndex"]) - 1;
        if (newind < 0)
        {
            newind = myColorpool.Count - 1;
        }
        PickedColorIndex(_player, newind, pickmode.Decrease);

    }
    public void OnPlayerBtnPressed()
    {
        PhotonNetwork.LoadLevel("game_scene");
    }
    public void OnExitBtnPressed()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("main_menu_scene");

    }

}
