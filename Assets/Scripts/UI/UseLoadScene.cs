using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseLoadScene : MonoBehaviour
{
    public GameObject LoadScreen;
    // Start is called before the first frame update
    public void UseLoadScreen()
    {
        LoadScreen.SetActive(true);
    }
}
