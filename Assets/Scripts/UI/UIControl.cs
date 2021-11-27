using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public static UIControl Instance;
    public Button _killBtn;
    public bool HasTarget;
    public Killable CurrentPlayer;

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        _killBtn.interactable = HasTarget;
    }
    public void OnKillButtonPressed()
    {
        if (CurrentPlayer == null) { return; }
        CurrentPlayer.Kill();
    }
}
