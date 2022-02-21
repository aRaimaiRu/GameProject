using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using RoleList;



public class Scanner : Role
{
    private GameObject MeetingActionBtn;
    // private Sprite MeetingActionBtnSprite;
    public RoleActionState CurrentAction;



    public override void Start()
    {
        base.Start();
        base._role = RoleListClass.RoleList.Scanner;
        base.hasGamePlayAction = false;
        base.hasMeetingAction = true;
        VotingManager.Instance.LocalPlayer = this;
        StartCoroutine(UIControl.Instance.DelayFadeThisWindow(UIControl.Instance.ScannerIntro));
        // MeetingActionBtnSprite = Resources.Load<Sprite>("kill-01");


        if (hasMeetingAction)
        {
            PrepareMeetingAction();

        }
        Debug.Log("Scanner Start");
    }

    public override void MeetingAction(int targetActorNumber)
    {
        base.MeetingAction(targetActorNumber);
        switch (CurrentAction)
        {
            case RoleActionState.Voting:
                VotingManager.Instance.ModeVote(targetActorNumber);
                break;
            case RoleActionState.ChoosePlayer:
                break;
                // currentTargetActorNumber = targetActorNumber;
                // VotingManager.Instance.DisablePlayerUIObj(targetActorNumber);

                // // VotingManager.Instance.ChooseRoleWindow?.SetActive(true);
                // CurrentAction = RoleActionState.ChooseRole;
                // VotingManager.Instance.onEndVote.AddListener(() => WormMeetingExecute(currentTargetActorNumber));
                // break;
        }
    }
    public override void PrepareMeetingAction()
    {
        base.PrepareMeetingAction();
    }


}



