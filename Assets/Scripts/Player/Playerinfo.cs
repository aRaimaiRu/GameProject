using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

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
    public void SetImpostor(MasterClient.Role _role)
    {
        switch (_role)
        {
            case MasterClient.Role.Spyware:
                this.gameObject.AddComponent<Spayware>();
                break;
            case MasterClient.Role.Worm:
                this.gameObject.AddComponent<Worm>();

                break;
        }

    }
}
