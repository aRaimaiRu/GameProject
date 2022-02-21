using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using RoleList;




public class Deleter : Impostor
{

    public override void Start()
    {
        base.Start();
        base._role = RoleListClass.RoleList.Deleter;
        base.hasGamePlayAction = true;
        base.hasMeetingAction = false;
    }

}



