using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using RoleList;




public class Deleter : Impostor
{

    public override void Start()
    {
        base.Start();
        base._role = RoleListClass.RoleList.Deleter;
        base.hasGamePlayAction = true;
        base.hasMeetingAction = false;
        if (!photonView.IsMine) { return; }
        StartCoroutine(UIControl.Instance.DelayFadeThisWindow(UIControl.Instance.DeleterIntro));

        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(CustomPropKey, (int)RoleListClass.RoleList.Deleter);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        VotingManager.Instance.LocalPlayer = this;
    }

}



