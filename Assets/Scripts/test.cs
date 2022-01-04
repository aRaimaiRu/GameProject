using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    void Start()
    {
        Debug.Log("version");
        Debug.Log(typeof(string).Assembly.ImageRuntimeVersion);



    }

    // Update is called once per frame
    void Update()
    {

    }
}
