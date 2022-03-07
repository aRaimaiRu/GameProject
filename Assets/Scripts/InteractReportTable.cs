using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractReportTable : Interactible
{
    public override void Use(bool isActive)
    {
        if (isActive != true) { return; }
        VotingManager.Instance.ReportBtn();
    }
}
