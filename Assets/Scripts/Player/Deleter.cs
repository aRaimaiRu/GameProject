using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;




public class Deleter : Role
{

    public override void Start()
    {
        base.Start();
        base.hasGamePlayAction = true;
        base.hasMeetingAction = false;
    }

}



