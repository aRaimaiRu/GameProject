using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class AbilityCooldownBtn : MonoBehaviour
{
    public float Cooldown = 5;
    public Timer _timer;
    [SerializeField] Image _image;
    [SerializeField] Button _button;
    [SerializeField] Text _text;
    private void OnEnable()
    {
        _timer = new Timer((float)Cooldown);
        StartTimer(Cooldown);
        _button.onClick.AddListener(() => StartTimer(Cooldown));
        _timer.OnTimerEnd += OnTimerEnd;
    }
    private void FixedUpdate()
    {
        _timer.Tick(Time.fixedDeltaTime);
        if (_timer.RemainingSeconds > 0)
        {
            _image.fillAmount = (_timer.RemainingSeconds / Cooldown);
        }
        else
        {
            _image.fillAmount = 0;
            // _button.interactable = true;

        }
        _text.text = ((int)_timer.RemainingSeconds).ToString();
    }
    public void OnTimerEnd()
    {
        _button.interactable = true;
        _text.gameObject.SetActive(false);

    }
    public void StartTimer(float _setTime)
    {
        _button.interactable = false;
        _timer.RemainingSeconds = Cooldown = _setTime;
        // Cooldown = _setTime;
        _text.gameObject.SetActive(true);

    }
    public void RestartTimer()
    {
        _button.interactable = false;
        _timer.RemainingSeconds = Cooldown;
        _text.gameObject.SetActive(true);
    }
}
