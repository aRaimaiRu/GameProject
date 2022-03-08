using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorArrangeTask : Task
{
    // random Core color
    // set color to 5 square
    // set square.nextsquare
    // check result
    [SerializeField] private List<ColorArrangeItem> colorArrangeItems;


    private void OnEnable()
    {
        List<int> randomList = new List<int> { 0, 0, 0 };
        int myRandom = Random.Range(0, randomList.Count);
        while (myRandom == 1)
        {
            myRandom = Random.Range(0, randomList.Count);
        }
        randomList[myRandom] = 1;
        colorArrangeItems = new List<ColorArrangeItem>(GetComponentsInChildren<ColorArrangeItem>());
        for (int i = 0; i < colorArrangeItems.Count; i++)
        {
            colorArrangeItems[i].gameObject.SetActive(true);
            colorArrangeItems[i].Setcolor(new Color(noMoreThan(((255f * randomList[0]) + (51 * i)), 255f), noMoreThan(((255f * randomList[1]) + (51 * i)), 255f), noMoreThan(((255f * randomList[2]) + (51 * i)), 255f), 255f));
            if (i == 0)
            {
                colorArrangeItems[i].NextItem = null;
            }
            else
            {
                colorArrangeItems[i].NextItem = colorArrangeItems[i - 1];

            }

            colorArrangeItems[i].isComplete = false;


        }
    }
    private float noMoreThan(float number, float target)
    {
        if (number > target)
        {
            return target / 255;
        }
        else
        {
            return number / 255;
        }

    }
    private void Update()
    {
        if (colorArrangeItems[0].isComplete)
        {
            base.OnComplete();
        }
    }

}
