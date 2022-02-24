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

        UIControl.Instance.IsImpostor = false;
        UIControl.Instance.SabotageBtn.SetActive(false);
        if (hasGamePlayAction)
        {
            GamePlayAction();
        }
        StartCoroutine(UIControl.Instance.DelayFadeThisWindow(UIControl.Instance.DeleterIntro));

        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(CustomPropKey, (int)RoleListClass.RoleList.Deleter);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        VotingManager.Instance.LocalPlayer = this;
    }

}



