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
    public enum RoleActionState { };
    protected RoleListClass.RoleList _role { get; set; }
    public RoleListClass.RoleList role
    {
        get { return _role; }
    }

    public virtual void Start()
    {
        if (!photonView.IsMine) { return; }

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



