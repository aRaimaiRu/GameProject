using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoBehaviourPun
{
    public static PlayerManager Instance;
    public List<Color> Colorlist;
    public struct PlayerStatus
    {
        public int Actornumber;
        public string PlayerName;
        public bool isDead;
        public int ColorIndex;
        public RoleList.RoleListClass.RoleList Role;
    }
    public List<PlayerStatus> PlayersStatus = new List<PlayerStatus>();
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        Instance = this;
    }
    private void Start()
    {
        TaskManager.Instance.OnPlayerKilledEvent += PlayerDead;
    }
    public void AddPlayer(int Actornumber, string PlayerName, bool isDead, int ColorIndex)
    {
        AddPlayerRPC(Actornumber, PlayerName, isDead, ColorIndex);
        // photonView.RPC("AddPlayerRPC", RpcTarget.All, Actornumber, PlayerName, isDead, ColorIndex);
    }
    [PunRPC]
    public void AddPlayerRPC(int Actornumber, string PlayerName, bool isDead, int ColorIndex)
    {
        PlayerStatus newPlayerStatus = new PlayerStatus();
        newPlayerStatus.Actornumber = Actornumber;
        newPlayerStatus.PlayerName = PlayerName;
        newPlayerStatus.isDead = isDead;
        newPlayerStatus.ColorIndex = ColorIndex;
        Debug.Log("player status = " + PlayersStatus);

        PlayersStatus.Add(newPlayerStatus);
        foreach (PlayerStatus p in PlayersStatus)
        {
            Debug.Log(p.PlayerName + " " + p.isDead);
        }
    }

    public void PlayerDead(int Actornumber)
    {
        PlayerDeadRPC(Actornumber);
        // photonView.RPC("AddPlayerRPC", RpcTarget.All, Actornumber);
    }
    [PunRPC]
    public void PlayerDeadRPC(int Actornumber)
    {
        PlayerStatus query = PlayersStatus.Find(x => x.Actornumber == Actornumber);
        query.isDead = true;
        PlayersStatus[PlayersStatus.IndexOf(PlayersStatus.Find(x => x.Actornumber == Actornumber))] = query;


        foreach (PlayerStatus p in PlayersStatus)
        {
            Debug.Log(p.PlayerName + "========= " + p.isDead);
        }

    }
    public void updateRole(int Actornumber, RoleList.RoleListClass.RoleList _role)
    {
        PlayerStatus query = PlayersStatus.Find(x => x.Actornumber == Actornumber);
        query.Role = _role;
        PlayersStatus[PlayersStatus.IndexOf(PlayersStatus.Find(x => x.Actornumber == Actornumber))] = query;
        foreach (PlayerStatus p in PlayersStatus)
        {
            Debug.Log(p.PlayerName + "========= " + p.Role);
        }
    }
    // เก็บ list คนที่ตายไปแล้วเพื่อที่จะเอา เลข actorNumber มาเช็คว่าคนนั้นตายรึยัง


}
