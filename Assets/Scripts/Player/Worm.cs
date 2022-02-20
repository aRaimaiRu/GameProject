using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using RoleList;
using Photon.Realtime;
public class Worm : Impostor
{
    private Sprite MeetingActionBtnSprite;
    public RoleActionState CurrentAction;
    private int currentTargetActorNumber;
    private GameObject MeetingActionBtn;
    private GameObject GameplayActionBtn;
    private List<int> markedActorNumber = new List<int>();

    public override void Start()
    {
        base.Start();
        base._role = RoleListClass.RoleList.Worm;
        if (!photonView.IsMine) { return; }
        VotingManager.Instance.LocalPlayer = this;

        base.hasMeetingAction = false;
        base.hasGamePlayAction = true;
        StartCoroutine(UIControl.Instance.DelayFadeThisWindow(UIControl.Instance.WormIntro));
        MeetingActionBtnSprite = Resources.Load<Sprite>("kill-01");

        if (hasGamePlayAction)
        {
            PrepareGamePlayAction();
        }
        if (hasMeetingAction)
        {
            PrepareMeetingAction();

        }
        Debug.Log("Worm Start");
    }
    public void PrepareGamePlayAction()
    {
        base.GamePlayAction();
        Debug.Log("Worm Action");
        markedActorNumber = new List<int>();
        GameplayActionBtn = UIControl.Instance.SpecialAbilityBtn;
        GameplayActionBtn.SetActive(true);
        GameplayActionBtn.GetComponent<Button>().onClick.AddListener(GamePlayActionFn);
        TaskManager.Instance.OnPlayerLeftRoomEvent += CheckMarkedPlayerLeftRoom;
    }
    public void GamePlayActionFn()
    {
        int _currentTargetActorNumber = _target.GetComponent<Playerinfo>().ActorNumber;
        if (!VotingManager.Instance.CheckIfPlayerIsImpostor(_currentTargetActorNumber))
        {
            markedActorNumber.Add(_currentTargetActorNumber);

        }
        if (markedActorNumber.Count == TaskManager.Instance.AntiVirusCount)
        {
            photonView.RPC("WormWinRPC", RpcTarget.MasterClient);
        }
    }
    public void CheckMarkedPlayerLeftRoom(int _ActorNumber)
    {
        if (markedActorNumber.Contains(_ActorNumber))
        {
            markedActorNumber.Remove(_ActorNumber);
        }

    }
    private void Update()
    {
        if (!photonView.IsMine && !GameplayActionBtn.activeSelf) { return; }

        GameplayActionBtn.GetComponent<Button>().interactable = markedActorNumber.Contains(_target.GetComponent<Playerinfo>().ActorNumber) ? false : UIControl.Instance._killBtn.interactable;

    }
    [PunRPC]
    public void WormWinRPC()
    {
        TaskManager.Instance.VirusWin();
    }


    // function when press button
    #region deprecate
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
        _targetPlayer.SetRole(RoleListClass.RoleList.Imposter);
    }
    #endregion



}



