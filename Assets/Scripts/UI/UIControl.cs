using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public static UIControl Instance;
    public Button _killBtn;
    public Button _useBtn;
    public Button _reportDeadBodyBtn;
    public bool HasTarget;
    public Killable CurrentPlayer;
    public bool HasInteractible;
    public Interactible CurrentInteractible;
    public bool HasDeadBodyInRange;
    public GameObject ChatWindowUI;
    public GameObject YouHaveBeenKilledWindow;
    public bool IsChatWindowActive { get { return ChatWindowUI.activeInHierarchy; } }

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
        _reportDeadBodyBtn.interactable = HasDeadBodyInRange;
    }
    public void OnKillButtonPressed()
    {
        if (CurrentPlayer == null) { return; }
        CurrentPlayer.Kill();
    }
    public void OnThisPlayerKilled()
    {
        // YouHaveBeenKilledWindow.SetActive(true);
        StartCoroutine(DelayFadeThisWindow(YouHaveBeenKilledWindow));
    }
    public void OnUseButtonPressed()
    {
        if (CurrentInteractible == null) { return; }
        CurrentInteractible.Use(true);
    }
    public void OnChatButtonPressed()
    {
        ChatWindowUI.SetActive(!ChatWindowUI.activeInHierarchy);
    }
    IEnumerator DelayFadeThisWindow(GameObject window)
    {
        window.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        window.SetActive(false);
    }


}
