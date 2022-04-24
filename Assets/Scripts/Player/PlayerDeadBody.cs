using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerDeadBody : Photon.Pun.MonoBehaviourPun
{
    [SerializeField] private List<SpriteRenderer> _bodyFill;

    public void SetColor(int index)
    {
        photonView.RPC("SetColorRPC", RpcTarget.All, index);

    }
    [PunRPC]
    public void SetColorRPC(int index)
    {
        foreach (SpriteRenderer _sr in _bodyFill)
        {
            _sr.color = PlayerManager.Instance.Colorlist[index];
        }
    }


}
