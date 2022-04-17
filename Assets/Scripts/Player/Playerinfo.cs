using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;
using RoleList;
public class Playerinfo : MonoBehaviourPun
{
    public int colorIndex;
    public SpriteRenderer playerBody;
    public List<SpriteRenderer> Bodyparts;
    public List<Color> _allPlayerColors = new List<Color>();
    public Color CurrentColor
    {
        get { return _allPlayerColors[colorIndex]; }
    }
    public Text _playerName;
    public GameObject SpawnPoint;
    private int _actorNumber;
    public int ActorNumber
    {
        get { return _actorNumber; }

    }
    private void Awake()
    {
        if (photonView.IsMine)
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("ColorIndex"))
            {
                colorIndex = (int)PhotonNetwork.LocalPlayer.CustomProperties["ColorIndex"];
                photonView.RPC("SetColorRPC", RpcTarget.All, colorIndex);

            }
            else
            {
                Debug.Log("No Color Index");
            }
        }
        else
        {
            Destroy(GameObject.FindGameObjectWithTag("MapMark"));
            // remove light of other player
            Destroy(GetComponentInChildren<Light2D>());
        }
        _playerName.text = GetPlayerName(photonView.OwnerActorNr);
        _actorNumber = photonView.OwnerActorNr;
    }
    private void Start()
    {
        VotingManager.Instance.onEndVote.AddListener(() => onThisVoteEnd());
    }
    private void onThisVoteEnd()
    {
        if (SpawnPoint != null)
        {
            transform.position = SpawnPoint.transform.position;
        }
    }

    [PunRPC]
    public void SetColorRPC(int _index)
    {
        colorIndex = _index;
        playerBody.color = CurrentColor;
        foreach (SpriteRenderer _sr in Bodyparts)
        {
            _sr.color = CurrentColor;
        }
    }



    // Update is called once per frame
    void Update()
    {
        playerBody.color = _allPlayerColors[colorIndex];
    }
    private string GetPlayerName(int actorID)
    {
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            if (player.Value.ActorNumber == actorID)
            {
                return player.Value.NickName;

            }

        }
        return "[none]";

    }
    [PunRPC]
    public void SetRole(RoleListClass.RoleList _role)
    {
        if (this.gameObject.GetComponent<Role>() != null)
        {
            Destroy(this.gameObject.GetComponent<Role>());
        }
        switch (_role)
        {
            case RoleListClass.RoleList.Spyware:
                this.gameObject.AddComponent<Spyware>();
                this.gameObject.AddComponent<Killable>();

                break;
            case RoleListClass.RoleList.Worm:
                this.gameObject.AddComponent<Worm>();
                this.gameObject.AddComponent<Killable>();

                break;
            case RoleListClass.RoleList.Process:
                this.gameObject.AddComponent<Killable>();
                this.gameObject.AddComponent<Role>();
                break;
            case RoleListClass.RoleList.Deleter:
                this.gameObject.AddComponent<Killable>();
                this.gameObject.AddComponent<Deleter>();

                break;
            case RoleListClass.RoleList.Scanner:
                this.gameObject.AddComponent<Killable>();
                this.gameObject.AddComponent<Scanner>();
                break;
            case RoleListClass.RoleList.Imposter:
                this.gameObject.AddComponent<Impostor>();
                this.gameObject.AddComponent<Killable>();

                break;
        }

    }
}
