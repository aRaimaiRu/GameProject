using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreItemTask : Task
{

    private List<StoreSlot> _storeslot;
    [SerializeField] List<RectTransform> storeItem;
    [SerializeField] List<RectTransform> spawnPoint;

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
    private void OnDisable()
    {
        for (int i = 0; i < storeItem.Count; i++)
        {
            storeItem[i].position = spawnPoint[i].position;
        }

    }

}
