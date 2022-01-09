using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MeetingSkillObjSpyware", menuName = "TestAmongUs/MeetingSkillObj/Spyware", order = 0)]
public class MeetingSkillObjSpyware : MeetingSkillObj
{
    public GameObject RoleWindow;
    // open Role Window
    // change VotingManager Mode
    public override void useSkill()
    {
        RoleWindow.SetActive(true);
        Debug.Log("Spyware meeting skill");
    }
}