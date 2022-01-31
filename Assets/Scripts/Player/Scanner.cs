using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;




public class Scanner : Role
{

    public override void Start()
    {
        base.Start();
        base.hasGamePlayAction = false;
        base.hasMeetingAction = true;
    }

}



