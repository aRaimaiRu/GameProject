using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewColorArrangeTask : StoreItemTask
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        List<int> randomList = new List<int> { 0, 0, 0 };
        int myRandom = Random.Range(0, randomList.Count);
        while (myRandom == 1)
        {
            myRandom = Random.Range(0, randomList.Count);
        }
        randomList[myRandom] = 1;
        for (int i = 0; i < storeItem.Count; i++)
        {
            storeItem[i].gameObject.SetActive(true);
            storeItem[i].GetComponent<RectTransform>().position = spawnPoint[i].position;
            storeItem[i].GetComponent<Image>().color = new Color(noMoreThan(((255f * randomList[0]) + (51 * i)), 255f), noMoreThan(((255f * randomList[1]) + (51 * i)), 255f), noMoreThan(((255f * randomList[2]) + (51 * i)), 255f), 255f);
            storeItem[i].GetComponent<StoreItem>().dragable = true;
            _storeslot[i].isfullfill = false;


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
        if (_storeslot.Find(x => x.isfullfill == false) != null)
        {
            return;
        }
        else
        {
            base.OnComplete();
        }
    }
}
