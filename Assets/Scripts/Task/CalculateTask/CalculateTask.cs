using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// start when Enable output when success this.Active(fault)
// random number and show
// randow choice and correct Number
// when click correct Number Successful()
// when click incorrect Number random New Choice()
public class CalculateTask : MonoBehaviour
{
    private bool isSuccessful;
    [SerializeField] private Text problem;

    [SerializeField] private List<GameObject> Choice;
    private int Answer;


    private void OnEnable()
    {
        isSuccessful = false;
        // random number1 + number 2
        int a = Random.Range(1, 1000);
        int b = Random.Range(1, 1000);
        Answer = a + b;
        problem.text = a.ToString() + " + " + b.ToString();
        for (int i = 0; i < Choice.Count; i++)
        {
            Choice[i].GetComponentInChildren<Text>().text = Random.Range((int)Answer / 2, (int)Answer * 2).ToString();

        }
        Choice[Random.Range(0, Choice.Count)].GetComponentInChildren<Text>().text = Answer.ToString();
    }
    public void CheckAnswer(int _answer)
    {
        if (Answer == _answer)
        {
            this.gameObject.SetActive(false);

        }
        else
        {
            OnEnable();

        }

    }

    private void Successful()
    {
        isSuccessful = true;
        this.gameObject.SetActive(false);
    }
}
