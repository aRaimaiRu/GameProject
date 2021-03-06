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

    public override void Start()
    {
        base.Start();
        base._role = RoleListClass.RoleList.Worm;
        PlayerManager.Instance.updateRole(photonView.OwnerActorNr, _role);

        if (!photonView.IsMine) { return; }
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(CustomPropKey, (int)RoleListClass.RoleList.Worm);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
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

        GameplayActionBtn = UIControl.Instance.SpecialAbilityBtn;
        GameplayActionBtn.SetActive(true);
        GameplayActionBtn.GetComponent<Button>().onClick.AddListener(GamePlayActionFn);

    }
    public void GamePlayActionFn()
    {
        int _currentTargetActorNumber = _target.GetComponent<Playerinfo>().ActorNumber;
        if (!VotingManager.Instance.CheckIfPlayerIsImpostor(_currentTargetActorNumber))
        {
            TaskManager.Instance.Addmarked(_currentTargetActorNumber);

        }
        TaskManager.Instance.CheckEnd();
    }

    private void Update()
    {
        if (!photonView.IsMine) { return; }
        if (!GameplayActionBtn.activeSelf) { return; }
        if (_target == null)
        {
            GameplayActionBtn.GetComponent<Button>().interactable = false;
            return;
        }
        GameplayActionBtn.GetComponent<Button>().interactable = TaskManager.Instance.markedActorNumber.Contains(_target.GetComponent<Playerinfo>().ActorNumber) ? false :
        GameplayActionBtn.GetComponent<AbilityCooldownBtn>()._timer.RemainingSeconds <= 0;

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



