using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MeetingSkillObj", menuName = "TestAmongUs/MeetingSkillObj", order = 0)]
public class MeetingSkillObj : ScriptableObject
{
    public Sprite btnImg;
    public virtual void useSkill()
    {

    }
}