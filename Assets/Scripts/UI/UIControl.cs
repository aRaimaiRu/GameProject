using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public static UIControl Instance;
    public Button _killBtn;
    public Button _useBtn;
    public bool HasTarget;
    public Killable CurrentPlayer;
    public bool HasInteractible;
    public Interactible CurrentInteractible;

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (CurrentPlayer != null)
        {
            _killBtn.gameObject.SetActive(CurrentPlayer.IsImpostor);
        }
        _killBtn.interactable = HasTarget;
        _useBtn.interactable = HasInteractible;
    }
    public void OnKillButtonPressed()
    {
        if (CurrentPlayer == null) { return; }
        CurrentPlayer.Kill();
    }
    public void OnUseButtonPressed()
    {
        if (CurrentInteractible == null) { return; }
        CurrentInteractible.Use(true);
    }
}
