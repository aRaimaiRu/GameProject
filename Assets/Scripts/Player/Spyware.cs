using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Spyware : Impostor
{
    public Sprite MeetingActionBtnSprite;
    private GameObject MeetingActionBtn;
    private GameObject SelectRoleWindow;
    public override void Start()
    {
        base.Start();
        base.hasMeetingAction = true;
        base.hasGamePlayAction = true;
        if (!photonView.IsMine) { return; }
        StartCoroutine(UIControl.Instance.DelayFadeThisWindow(UIControl.Instance.SpywareIntro));

        if (hasGamePlayAction)
        {
            GamePlayAction();

        }
        if (hasMeetingAction)
        {
            MeetingAction();

        }
        Debug.Log("Spyware Start");
    }
    public override void GamePlayAction()
    {
        base.GamePlayAction();

        Debug.Log("Spyware Action");
    }
    // set image button and function to button
    public override void MeetingAction()
    {
        base.MeetingAction();

        SelectRoleWindow = GameObject.Find("CanvasOverlay/NewVoteWindow/ChooseRoleWindow");

        MeetingActionBtn = UIControl.Instance.MeetingSkillBtn;
        MeetingActionBtn.SetActive(true);
        MeetingActionBtn.GetComponent<Image>().sprite = MeetingActionBtnSprite;
        MeetingActionBtn.GetComponent<Button>().onClick.AddListener(useMeetingAction);
    }
    public void useMeetingAction()
    {
        GameObject.Find("CanvasOverlay/NewVoteWindow/ChooseRoleWindow").SetActive(true);

    }
    // prepate image to button
    // when click button swap to mode choose player
    // when choose player open Window choose Role
    // when choose Role check if player role == choose role
    // kill player


}



