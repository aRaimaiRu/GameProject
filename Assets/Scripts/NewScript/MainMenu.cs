using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class MainMenu : MonoBehaviourPunCallbacks
{

    // Player Name Functionality
    [SerializeField] private InputField _playerNameInput;
    [SerializeField] private Text _playerNameLabel;
    private bool _isPlayerNameChanging = false;




    void Start()
    {

        Connect();
    }
    private void Connect()
    {
        PhotonNetwork.NickName = "Player" + Random.Range(0, 5000);
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Master");
        // give random player name
        PhotonNetwork.NickName = "Player" + Random.Range(0, 5000);
        _playerNameLabel.text = PhotonNetwork.NickName;
        PhotonNetwork.JoinLobby();
    }
    // private void OnFailedToConnectToMasterServer(NetworkConnectionError error)
    // {
    //     Debug.Log("Failed to Connect Master Server" + error);
    // }
    public void OnChangePlayerNamePressed()
    {
        Debug.Log("_isPlayerNameChanging =" + _isPlayerNameChanging);
        if (_isPlayerNameChanging == false)
        {
            _playerNameInput.text = _playerNameLabel.text;
            _playerNameLabel.gameObject.SetActive(false);
            _playerNameInput.gameObject.SetActive(true);
            _isPlayerNameChanging = true;

        }
        else
        {
            // check for empty or long names
            if (string.IsNullOrEmpty(_playerNameInput.text) == false && _playerNameInput.text.Length <= 12)
            {
                _playerNameLabel.text = _playerNameInput.text;
                PhotonNetwork.LocalPlayer.NickName = _playerNameInput.text;
                // photonView.RPC("ForcePlayerListUpdate", RpcTarget.All);
            }
            _playerNameLabel.gameObject.SetActive(true);
            _playerNameInput.gameObject.SetActive(false);
            _isPlayerNameChanging = false;
        }
    }







}
