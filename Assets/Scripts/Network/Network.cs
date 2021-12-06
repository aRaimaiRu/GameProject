using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class Network : MonoBehaviourPunCallbacks
{
    public MasterClient masterClient;
    public Text StatusText;
    public CameraFollow PlayerCamera;
    public ChatWindowUI chatWindowUI;
    public UIControl uIControl;
    public VotingManager votingManager;
    // Start is called before the first frame update
    void Start()
    {
        // StatusText.text = "Connecting";
        // PhotonNetwork.NickName = "Player " + Random.Range(0, 20);
        // PhotonNetwork.ConnectUsingSettings();
        GameObject newPlayer = PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(0, 5), Random.Range(0, 5), 0), Quaternion.identity);
        PlayerCamera.target = newPlayer.transform;
        chatWindowUI._playerInfo = newPlayer.GetComponent<Playerinfo>();
        newPlayer.GetComponent<Move>()._uiControl = uIControl;
        newPlayer.GetComponentInChildren<PlayerDeadBodyReport>().Initialize(uIControl, votingManager);
        // newPlayer.GetComponentInChildren<PlayerDeadBodyReport>().initialize(uIControl,votingManager);

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Initialize");
            masterClient.Initialize();

        }
    }
    // public override void OnConnectedToMaster()
    // {
    //     StatusText.text = "Connected To Master";
    //     PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions() { MaxPlayers = 4 }, null);

    // }
    // public override void OnJoinedRoom()
    // {
    //     StatusText.text = "Connected";
    //     if (PlayerCamera == null) return;
    //     PlayerCamera.target = PhotonNetwork.Instantiate("Player", new Vector3(Random.Range(0, 5), Random.Range(0, 5), 0), Quaternion.identity).transform;
    // }


}
