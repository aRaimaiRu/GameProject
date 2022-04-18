using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AbilityCooldownBtn))]
public class AbilityCooldownBtnResetOnVote : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        VotingManager.Instance.onEndVote.AddListener(() =>
        {
            GetComponent<AbilityCooldownBtn>().RestartTimer();
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
