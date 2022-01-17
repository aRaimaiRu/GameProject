using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreItemTask : MonoBehaviour
{
    public enum ItemType
    {
        Square,
        Circle,
        Hexgagon
    }
    private List<StoreSlot> _storeslot;
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
            this.gameObject.SetActive(false);
        }
    }

}
