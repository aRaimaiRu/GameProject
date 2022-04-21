using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Ghost : Photon.Pun.MonoBehaviourPun, IPunObservable
{
    [SerializeField] private Text playerName;
    [SerializeField] private SpriteRenderer _bodyFill;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // owner
            stream.SendNext(_bodyFill.color.r);
            stream.SendNext(_bodyFill.color.g);
            stream.SendNext(_bodyFill.color.b);
        }
        else
        {
            float red = (float)stream.ReceiveNext();
            float green = (float)stream.ReceiveNext();
            float blue = (float)stream.ReceiveNext();
            _bodyFill.color = new Color(red, green, blue, 1.0f);
        }
    }

    public void SetColor(Color color)
    {
        _bodyFill.color = color;

    }
    public void SetPlayerNameColor(int color)
    {
        Debug.Log("SetPlayerNameColor =" + color);
        photonView.RPC("SetPlayerNameColorRPC", RpcTarget.All, color);
    }
    [PunRPC]
    public void SetPlayerNameColorRPC(int color)
    {
        playerName.color = color == 1 ? Color.red : Color.black;
        playerName.material = null;
    }
    void Start()
    {
        if (TaskManager.Instance == null) { return; }
        playerName.text = GetPlayerName(photonView.OwnerActorNr);
        TaskManager.Instance.OnPlayerKilledTrigger(photonView.OwnerActorNr);
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
}
