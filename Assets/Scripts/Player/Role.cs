using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using RoleList;



public class Role : MonoBehaviourPun
{
    public bool hasMeetingAction;
    public bool hasGamePlayAction;
    public enum RoleActionState
    {
        Voting,
        ChoosePlayer,
        ChooseRole,
        Execute
    };
    protected RoleListClass.RoleList _role { get; set; }
    protected string CustomPropKey = "Role";

    public RoleListClass.RoleList role
    {
        get { return _role; }
    }


    public virtual void Start()
    {
        if (!photonView.IsMine) { return; }
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(CustomPropKey, (int)RoleListClass.RoleList.Process);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        VotingManager.Instance.LocalPlayer = this;
        GameObject.FindObjectOfType<TaskManager>().Initialize();
    }
    public virtual void MeetingAction(int targeActorNumber)
    {

    }
    public virtual void GamePlayAction()
    {

    }
    public virtual void PrepareMeetingAction()
    {

    }



}



