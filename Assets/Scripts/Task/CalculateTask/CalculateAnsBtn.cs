using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CalculateAnsBtn : MonoBehaviour
{
    private void OnEnable()
    {
        this.GetComponent<Button>().onClick.AddListener(() => SendAnswer());

    }
    public void SendAnswer()
    {
        this.GetComponentInParent<CalculateTask>().CheckAnswer(int.Parse(this.GetComponentInChildren<Text>().text.Trim()));
    }
}
