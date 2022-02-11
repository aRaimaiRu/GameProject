using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using RoleList;
public partial class Spyware : Impostor
{
    private Sprite MeetingActionBtnSprite;
    private GameObject MeetingActionBtn;
    private GameObject SelectRoleWindow;
    private GameObject RoleChooseWindow;

    public RoleActionState CurrentAction;
    private int currentTargetActorNumber;
    public override void Start()
    {
        base.Start();
        base._role = RoleListClass.RoleList.Spyware;
        base.hasMeetingAction = true;
        base.hasGamePlayAction = true;
        if (!photonView.IsMine) { return; }
        StartCoroutine(UIControl.Instance.DelayFadeThisWindow(UIControl.Instance.SpywareIntro));
        MeetingActionBtnSprite = Resources.Load<Sprite>("kill-01");
        // RoleChooseWindow = GameObject.Find("CanvasOverlay/NewVoteWindow/ChooseRoleWindow");

        if (hasGamePlayAction)
        {
            GamePlayAction();

        }
        if (hasMeetingAction)
        {
            PrepareMeetingAction();

        }
        Debug.Log("Spyware Start");
    }
    public override void GamePlayAction()
    {
        base.GamePlayAction();

        Debug.Log("Spyware Action");
    }
    // set image button and function to button
    public override void MeetingAction(int targetActorNumber)
    {
        base.MeetingAction(targetActorNumber);
        switch (CurrentAction)
        {
            case RoleActionState.Voting:
                VotingManager.Instance.ModeVote(targetActorNumber);
                break;
            case RoleActionState.ChoosePlayer:
                currentTargetActorNumber = targetActorNumber;
                VotingManager.Instance.ChooseRoleWindow?.SetActive(true);
                CurrentAction = RoleActionState.ChooseRole;
                VotingManager.Instance.onChooseRole.AddListener(() => SpywareMeetingExecute(VotingManager.Instance.CurrentChooseRole));
                break;
            case RoleActionState.ChooseRole:
                // Check targetActorNumber and Role that choosen
                // Rolebutton.onClick =  SpywareMeetingKill(Role)
                // CurrentAction = RoleActionState.Voting;

                break;
        }

    }
    public void SpywareMeetingExecute(RoleListClass.RoleList _role)
    {
        Debug.Log("role =" + _role + " Current target player =" + currentTargetActorNumber);
        if (_role == VotingManager.Instance.CheckRoleOfPlayer(currentTargetActorNumber))
        {
            VotingManager.Instance.KillInMeeting(currentTargetActorNumber);
            // photonView.RPC("KillInMeeting", RpcTarget.All, currentTargetActorNumber);
        }
        else
        {
            VotingManager.Instance.KillInMeeting(PhotonNetwork.LocalPlayer.ActorNumber);

        }
        VotingManager.Instance.ChooseRoleWindow?.SetActive(false);


    }
    public void useMeetingAction()
    {
        // RoleChooseWindow.SetActive(true);
        // VotingManager.Instance.CurrentAction = VotingManager.playerAction.ChooseRole;
        this.CurrentAction = RoleActionState.ChoosePlayer;


    }
    public override void PrepareMeetingAction()
    {
        base.PrepareMeetingAction();
        SelectRoleWindow = GameObject.Find("CanvasOverlay/NewVoteWindow/ChooseRoleWindow");
        CurrentAction = RoleActionState.Voting;
        VotingManager.Instance.LocalPlayer = this;


        MeetingActionBtn = UIControl.Instance.MeetingSkillBtn;
        MeetingActionBtn.SetActive(true);
        MeetingActionBtn.GetComponent<Image>().sprite = MeetingActionBtnSprite;
        MeetingActionBtn.GetComponent<Button>().onClick.AddListener(useMeetingAction);
    }

    // prepate image to button
    // when click button swap to mode choose player
    // when choose player open Window choose Role
    // when choose Role check if player role == choose role
    // kill player


}



