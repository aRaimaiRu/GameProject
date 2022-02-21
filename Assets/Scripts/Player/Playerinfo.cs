using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;
using RoleList;
public class Playerinfo : Photon.Pun.MonoBehaviourPun, IPunObservable
{
    public int colorIndex;
    public SpriteRenderer playerBody;
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
            colorIndex = (int)PhotonNetwork.LocalPlayer.CustomProperties["ColorIndex"];
            _playerName.text = PhotonNetwork.LocalPlayer.NickName;
        }
        else
        {
            _playerName.text = GetPlayerName(photonView.OwnerActorNr);
            // remove light of other player
            Destroy(GetComponentInChildren<Light2D>());
        }
        _actorNumber = photonView.OwnerActorNr;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(colorIndex);
            Debug.Log("Writing" + colorIndex);
        }
        else
        {
            colorIndex = (int)stream.ReceiveNext();
            Debug.Log("Not Writing" + colorIndex);
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
