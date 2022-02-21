using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeetingSkillBtn : MonoBehaviour
{
    public MeetingSkillObj SO;
    public Image img;

    public void Initialize()
    {
        if (SO == null) { return; }
        this.gameObject.SetActive(true);
        img.sprite = SO.btnImg;
        this.gameObject.GetComponent<Button>().onClick.AddListener(SO.useSkill);

    }

}
