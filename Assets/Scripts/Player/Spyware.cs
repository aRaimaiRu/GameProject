using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;



public class Spyware : Impostor
{
    new bool hasMeetingAction = true;
    new bool hasGamePlayAction = true;

    public override void Start()
    {
        base.Start();
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


}



