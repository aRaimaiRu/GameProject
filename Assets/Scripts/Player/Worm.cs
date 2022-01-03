using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;



public class Worm : Impostor
{
    new bool hasMeetingAction = true;
    new bool hasGamePlayAction = true;
    public override void Start()
    {
        base.Start();
        if (hasGamePlayAction)
        {
            GamePlayAction();
        }
        Debug.Log("Worm Start");
    }
    public override void GamePlayAction()
    {
        base.GamePlayAction();
        Debug.Log("Worm Action");
    }


}



