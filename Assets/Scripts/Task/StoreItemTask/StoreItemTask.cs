using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreItemTask : Task
{

    public List<StoreSlot> _storeslot;
    public List<RectTransform> storeItem;
    public List<RectTransform> spawnPoint;

    private void Awake()
    {
        _storeslot = new List<StoreSlot>(GetComponentsInChildren<StoreSlot>());
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
    private void OnEnable()
    {
        for (int i = 0; i < storeItem.Count; i++)
        {
            _storeslot[i].isfullfill = false;
            storeItem[i].position = spawnPoint[i].position;
        }

    }

}
