using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Experimental.Rendering.Universal;
using Util;
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
    [SerializeField] private List<GameObject> spawnPoints;
    public int ThisPlayerNumber;

    // Start is called before the first frame update
    void Start()
    {


        // StatusText.text = "Connecting";
        // PhotonNetwork.NickName = "Player " + Random.Range(0, 20);
        // PhotonNetwork.ConnectUsingSettings();
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i] == PhotonNetwork.LocalPlayer)
            {
                ThisPlayerNumber = i;
            }

        }
        Debug.Log("ThisPlayerNumber =" + ThisPlayerNumber);
        GameObject newPlayer = PhotonNetwork.Instantiate("Player", spawnPoints[ThisPlayerNumber].transform.position, Quaternion.identity);
        PlayerCamera.target = newPlayer.transform;
        chatWindowUI._playerInfo = newPlayer.GetComponent<Playerinfo>();
        newPlayer.GetComponent<Playerinfo>().SpawnPoint = spawnPoints[ThisPlayerNumber];
        // remove spawnpoint
        // photonView.RPC("RemoveSpawnPoint", RpcTarget.All, spawnPoints[ThisPlayerNumber]);



        newPlayer.GetComponent<Move>()._uiControl = uIControl;
        newPlayer.GetComponentInChildren<PlayerDeadBodyReport>().Initialize(uIControl, votingManager);
        _playerPhotonView = newPlayer.GetComponent<PhotonView>();

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Initialize");
            masterClient.Initialize();

        }
    }
    public void DestroyPlayer()
    {
        Debug.Log("Destroy player ActorNumber :" + PhotonNetwork.LocalPlayer.ActorNumber);
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
        if (_playerPhotonView)
        {
            PhotonNetwork.Destroy(_playerPhotonView);

        }
        VotingManager.Instance.CheckEndByVote();
    }
    [PunRPC]
    public void RemoveSpawnPoint(GameObject _spawnPoint)
    {
        spawnPoints.Remove(_spawnPoint);
    }






}
