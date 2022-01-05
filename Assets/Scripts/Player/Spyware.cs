using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;



public class Spyware : Impostor
{


    public override void Start()
    {
        base.Start();
        base.hasMeetingAction = true;
        base.hasGamePlayAction = true;
        if (photonView.IsMine)
        {
            StartCoroutine(UIControl.Instance.DelayFadeThisWindow(UIControl.Instance.SpywareIntro));

        }

        if (hasGamePlayAction)
        {
            GamePlayAction();
        }
        Debug.Log("Spyware Start");
    }
    public override void GamePlayAction()
    {
        base.GamePlayAction();

        Debug.Log("Spyware Action");
    }
    public override void MeetingAction()
    {
        base.MeetingAction();
    }


}



