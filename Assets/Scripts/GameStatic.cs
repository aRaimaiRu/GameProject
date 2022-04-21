using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatic : MonoBehaviour
{
    public static GameStatic Instance;
    public PlayerManager playerManager;
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
