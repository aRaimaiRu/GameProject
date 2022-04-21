using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Experimental.Rendering.Universal;
using Util;
using Cinemachine;
public class Network : MonoBehaviourPunCallbacks
{
    public MasterClient masterClient;
    public CameraFollow PlayerCamera;
    public CinemachineVirtualCamera NewPlayerCamera;
    public ChatWindowUI chatWindowUI;
    public UIControl uIControl;
    public VotingManager votingManager;
    private PhotonView _playerPhotonView;
    [SerializeField] private Camera MainCamera;
    [SerializeField] private Light2D GlobalLight;
    [SerializeField] private Light2D NewGlobalLight;

    [SerializeField] private LayerMask CullingMaskAfterDeath;
    public List<GameObject> spawnPoints;
    public List<GameObject> EndGameSpawnPoints;
    public int ThisPlayerNumber;
    [SerializeField] private GameObject GhostPrefab;
    [SerializeField] private GameObject ProcessPrefab;


    // Start is called before the first frame update
    void Start()
    {
        // StatusText.text = "Connecting";
        // PhotonNetwork.NickName = "Player " + Random.Range(0, 20);
        // PhotonNetwork.ConnectUsingSettings();
        VotingManager.Instance.onEndVote.AddListener(() => TeleportAllplayerOnVoteEnd());
        TaskManager.Instance.onAntiVirusWin += TeleportAllplayerOnVoteGameEnd;
        TaskManager.Instance.onVirusWin += TeleportAllplayerOnVoteGameEnd;
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i] == PhotonNetwork.LocalPlayer)
            {
                ThisPlayerNumber = i;
            }

        }
        Debug.Log("ThisPlayerNumber =" + ThisPlayerNumber);
        GameObject newPlayer = PhotonNetwork.Instantiate("TestPlayer", spawnPoints[ThisPlayerNumber].transform.position, Quaternion.identity);
        // PlayerCamera.target = newPlayer.transform;
        NewPlayerCamera.Follow = newPlayer.transform;
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
        // PlayerCamera.target = GhostPlayer.transform;
        NewPlayerCamera.Follow = GhostPlayer.transform;
        chatWindowUI._playerInfo = GhostPlayer.GetComponent<Playerinfo>();
        GhostPlayer.GetComponent<Move>()._uiControl = uIControl;

        GhostPlayer.GetComponent<Ghost>().SetColor(playerinfo._allPlayerColors[playerinfo.colorIndex]);
        Debug.Log("ghost is Virus = " + RoleList.RoleListClass.VirusRoleList.Contains(playerinfo.GetComponent<Role>().role));
        GhostPlayer.GetComponent<Ghost>().SetPlayerNameColor(RoleList.RoleListClass.VirusRoleList.Contains(playerinfo.GetComponent<Role>().role) ? 1 : 0);

        // see like ghost
        MainCamera.cullingMask = CullingMaskAfterDeath.value;
        GlobalLight.gameObject.SetActive(false);
        NewGlobalLight.gameObject.SetActive(true);


        if (_playerPhotonView)
        {
            PhotonNetwork.Destroy(_playerPhotonView);
        }

    }
    public void TeleportAllplayerOnVoteEnd()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        int i = 0;
        foreach (Move m in FindObjectsOfType<Move>())
        {
            m.photonView.RPC("TeleportRPC", RpcTarget.All, spawnPoints[i % spawnPoints.Count].transform.position);
            i++;
        }

    }
    public void TeleportAllplayerOnVoteGameEnd()
    {
        // if (!PhotonNetwork.IsMasterClient) return;
        // int i = 0;
        // foreach (Move m in FindObjectsOfType<Move>())
        // {
        //     Debug.Log("TeleportRPC =" + FindObjectsOfType<Move>().Length);
        //     m.photonView.RPC("TeleportRPC", RpcTarget.All, EndGameSpawnPoints[i % EndGameSpawnPoints.Count].transform.position);
        //     Destroy(m);
        //     i++;
        // }
        // foreach (Playerinfo p in FindObjectsOfType<Playerinfo>())
        // {
        //     p.photonView.RPC("SetPlayerNameColorRPC", RpcTarget.All, RoleList.RoleListClass.VirusRoleList.Contains(p.GetComponent<Role>().role) ? 1 : 0);
        // }
        Debug.Log("Teleport All player on Vote Game End =========> " + PlayerManager.Instance.PlayersStatus.Count);
        int i = 0;
        foreach (PlayerManager.PlayerStatus _playerStatus in PlayerManager.Instance.PlayersStatus)
        {
            GameObject GO = Instantiate(_playerStatus.isDead ? GhostPrefab : ProcessPrefab, EndGameSpawnPoints[i % EndGameSpawnPoints.Count].transform.position, Quaternion.identity);
            GO.GetComponent<DummyCharacter>().initlialize(PlayerManager.Instance.Colorlist[_playerStatus.ColorIndex], _playerStatus.PlayerName, RoleList.RoleListClass.VirusRoleList.Contains(_playerStatus.Role) ? Color.red : Color.black);
            i++;
        }

    }
    [PunRPC]
    public void RemoveSpawnPoint(GameObject _spawnPoint)
    {
        spawnPoints.Remove(_spawnPoint);
    }






}
