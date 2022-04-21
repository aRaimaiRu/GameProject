using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using RoleList;



public class Scanner : Role
{
    private GameObject MeetingActionBtn;

    private Sprite MeetingActionBtnSprite;
    public RoleActionState CurrentAction;



    public override void Start()
    {
        base.Start();
        base._role = RoleListClass.RoleList.Scanner;
        base.hasGamePlayAction = false;
        base.hasMeetingAction = true;
        PlayerManager.Instance.updateRole(photonView.OwnerActorNr, _role);

        if (!photonView.IsMine) return;
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add(CustomPropKey, (int)RoleListClass.RoleList.Scanner);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        // 
        VotingManager.Instance.LocalPlayer = this;
        StartCoroutine(UIControl.Instance.DelayFadeThisWindow(UIControl.Instance.ScannerIntro));
        MeetingActionBtnSprite = Resources.Load<Sprite>("kill-01");


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
                VotingManager.Instance.ShowPlayerRole(targetActorNumber);
                this.CurrentAction = RoleActionState.Voting;

                MeetingActionBtn.SetActive(false);
                break;

        }
    }
    public override void PrepareMeetingAction()
    {
        base.PrepareMeetingAction();
        MeetingActionBtn = UIControl.Instance.MeetingSkillBtn;
        MeetingActionBtn.SetActive(true);
        MeetingActionBtn.GetComponent<Image>().sprite = VotingManager.RoleSkillSymbol[RoleListClass.RoleList.Scanner];
        MeetingActionBtn.GetComponent<Button>().onClick.AddListener(useMeetingAction);
    }
    private void useMeetingAction()
    {
        this.CurrentAction = RoleActionState.ChoosePlayer;

    }



}



