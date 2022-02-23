using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Killable : Photon.Pun.MonoBehaviourPun
{

    private void Awake()
    {
        if (!photonView.IsMine) { return; }

    }
    private void Start()
    {
        if (!photonView.IsMine) { return; }


    }


    public void Kill()
    {
        Debug.Log("Use kill RPC ?");
        photonView.RPC("KillRPC", RpcTarget.All);

    }

    [PunRPC]
    public void KillRPC()
    {
        Debug.Log("Kill RPC photonView.OwnerActorNr =" + photonView.OwnerActorNr);
        if (!photonView.IsMine) { return; }
        Debug.Log("Use kill RPC ");

        PlayerDeadBody playerBody = PhotonNetwork.Instantiate("PlayerBody", this.transform.position, Quaternion.identity).GetComponent<PlayerDeadBody>();
        Playerinfo playerinfo = GetComponent<Playerinfo>();
        // 
        playerBody.SetColor(playerinfo._allPlayerColors[playerinfo.colorIndex]);

        GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Network>().DestroyPlayer();

        UIControl.Instance.OnThisPlayerKilled();
    }


}
