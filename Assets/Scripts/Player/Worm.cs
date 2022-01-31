using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Worm : Impostor
{
    private Sprite MeetingActionBtnSprite;

    new public enum RoleActionState
    {
        Voting,
        ChoosePlayer,
        ChooseRole,
        Execute


    }

    public RoleActionState CurrentAction;
    private int currentTargetActorNumber;
    private GameObject MeetingActionBtn;


    public override void Start()
    {
        base.Start();
        base._role = MasterClient.Role.Worm;
        if (!photonView.IsMine) { return; }
        VotingManager.Instance.LocalPlayer = this;

        base.hasMeetingAction = true;
        base.hasGamePlayAction = true;
        StartCoroutine(UIControl.Instance.DelayFadeThisWindow(UIControl.Instance.WormIntro));
        MeetingActionBtnSprite = Resources.Load<Sprite>("kill-01");

        if (hasGamePlayAction)
        {
            GamePlayAction();
        }
        if (hasMeetingAction)
        {
            PrepareMeetingAction();

        }
        Debug.Log("Worm Start");
    }
    public override void GamePlayAction()
    {
        base.GamePlayAction();
        Debug.Log("Worm Action");
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
                currentTargetActorNumber = targetActorNumber;
                VotingManager.Instance.DisablePlayerUIObj(targetActorNumber);

                // VotingManager.Instance.ChooseRoleWindow?.SetActive(true);
                CurrentAction = RoleActionState.ChooseRole;
                VotingManager.Instance.onEndVote.AddListener(() => WormMeetingExecute(currentTargetActorNumber));
                break;
        }
    }
    public override void PrepareMeetingAction()
    {
        base.PrepareMeetingAction();
        CurrentAction = RoleActionState.Voting;
        VotingManager.Instance.LocalPlayer = this;


        MeetingActionBtn = UIControl.Instance.MeetingSkillBtn;
        MeetingActionBtn?.SetActive(true);
        MeetingActionBtn.GetComponent<Image>().sprite = MeetingActionBtnSprite;
        MeetingActionBtn.GetComponent<Button>().onClick.AddListener(useMeetingAction);
    }
    public void useMeetingAction()
    {
        this.CurrentAction = RoleActionState.ChoosePlayer;
    }
    private void WormMeetingExecute(int _currentTargetActorNumber)
    {
        photonView.RPC("WormMeetingExecuteRPC", RpcTarget.All, _currentTargetActorNumber);
    }
    [PunRPC]
    public void WormMeetingExecuteRPC(int _currentTargetActorNumber)
    {
        List<Playerinfo> allplayerinfo = new List<Playerinfo>(FindObjectsOfType<Playerinfo>());
        Playerinfo _targetPlayer = allplayerinfo.Find(x => x.ActorNumber == _currentTargetActorNumber);
        _targetPlayer.SetRole(MasterClient.Role.Imposter);
    }


}



