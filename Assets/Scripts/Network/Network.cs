using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Experimental.Rendering.Universal;

public class Network : MonoBehaviourPunCallbacks
{
    public MasterClient masterClient;
    public Text StatusText;
    public CameraFollow PlayerCamera;
    public ChatWindowUI chatWindowUI;
    public UIControl uIControl;
    public VotingManager votingManager;
    private PhotonView _playerPhotonView;
    [SerializeField] private Camera MainCamera;
    [SerializeField] private Light2D GlobalLight;
    [SerializeField] private Light2D NewGlobalLight;

    [SerializeField] private LayerMask CullingMaskAfterDeath;


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
        _playerPhotonView = newPlayer.GetComponent<PhotonView>();

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Initialize");
            masterClient.Initialize();

        }
    }
    public void DestroyPlayer()
    {
        // spawn body
        // PlayerDeadBody playerBody = PhotonNetwork.Instantiate("PlayerBody", this.transform.position, Quaternion.identity).GetComponent<PlayerDeadBody>();
        Playerinfo playerinfo = _playerPhotonView.GetComponent<Playerinfo>();
        // playerBody.SetColor(playerinfo._allPlayerColors[playerinfo.colorIndex]);
        // spawn ghost
        GameObject GhostPlayer = PhotonNetwork.Instantiate("ghost", _playerPhotonView.transform.position, Quaternion.identity);
        PlayerCamera.target = GhostPlayer.transform;
        chatWindowUI._playerInfo = GhostPlayer.GetComponent<Playerinfo>();
        GhostPlayer.GetComponent<Move>()._uiControl = uIControl;
        GhostPlayer.GetComponent<Ghost>().SetColor(playerinfo._allPlayerColors[playerinfo.colorIndex]);
        // see like ghost
        MainCamera.cullingMask = CullingMaskAfterDeath.value;
        GlobalLight.gameObject.SetActive(false);
        NewGlobalLight.gameObject.SetActive(true);

        // 
        PhotonNetwork.Destroy(_playerPhotonView);
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
