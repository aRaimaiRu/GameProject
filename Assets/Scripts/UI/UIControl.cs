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
    // public Killable CurrentPlayer;
    public bool HasInteractible;
    public Interactible CurrentInteractible;
    public bool HasDeadBodyInRange;
    public GameObject ChatWindowUI;
    public GameObject YouHaveBeenKilledWindow;
    public bool IsChatWindowActive { get { return ChatWindowUI.activeInHierarchy; } }
    public bool IsImpostor = false;
    public GameObject ProcessIntro;
    public GameObject ScannerIntro;
    public GameObject DeleterIntro;
    public GameObject WormIntro;
    public GameObject SpywareIntro;
    public GameObject MeetingSkillBtn;
    public GameObject SabotageBtn;
    public GameObject SpecialAbilityBtn;
    public GameObject MapTexture;

    [SerializeField] GameObject WinScreen;
    [SerializeField] GameObject LoseScreen;



    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        TaskManager.Instance.onAntiVirusWin += AnitiVirusWin;
        TaskManager.Instance.onVirusWin += VirusWin;

    }
    private void Update()
    {
        if (_killBtn.gameObject.activeSelf)
        {
            _killBtn.interactable = _killBtn.GetComponent<AbilityCooldownBtn>()._timer.RemainingSeconds == 0 ? HasTarget : false;

        }



        _useBtn.interactable = HasInteractible;
        _reportDeadBodyBtn.interactable = HasDeadBodyInRange;
    }

    public void ImpostorBtn()
    {
        _killBtn.gameObject.SetActive(true);
        SabotageBtn.gameObject.SetActive(true);
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
    public IEnumerator DelayFadeThisWindow(GameObject window)
    {
        window.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        window.SetActive(false);
    }

    private void AnitiVirusWin()
    {
        if (IsImpostor)
        {
            ShowLoseScreeen();
        }
        else
        {
            ShowWinScreen();
        }
    }
    private void VirusWin()
    {
        if (IsImpostor)
        {
            ShowWinScreen();
        }
        else
        {
            ShowLoseScreeen();
        }
    }
    private void ShowWinScreen()
    {
        WinScreen.SetActive(true);
    }
    private void ShowLoseScreeen()
    {
        LoseScreen.SetActive(true);
    }
    public void ShowMap()
    {
        MapTexture.SetActive(!MapTexture.activeSelf);
    }




}
