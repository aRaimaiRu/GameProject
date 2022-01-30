using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;




public class Role : MonoBehaviourPun
{
    public bool hasMeetingAction;
    public bool hasGamePlayAction;
    public enum RoleActionState { };
    protected MasterClient.Role _role { get; set; }
    public MasterClient.Role role
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



