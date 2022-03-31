using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Image BG;
    [SerializeField] AudioSource bgm;
    [SerializeField] AudioSource sfx;


    public Sound[] sounds;

    public static AudioManager instance;


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            // s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    private void Start()
    {
        bgmSlider.onValueChanged.AddListener((float value) => setVolume("bgm", value));
        sfxSlider.onValueChanged.AddListener((float value) => setVolume("sfx", value));


        bgmSlider.value = bgm.volume;
        sfxSlider.value = sfx.volume;

    }

    public void Play(string name)
    {
        Sound s = Array.Find<Sound>(sounds, sound => sound.name == name);
        if (s == null) return;
        s.source.Play();
    }

    public void setVolume(string volumeType, float value)
    {
        Sound[] mysounds = Array.FindAll<Sound>(sounds, sound => sound.source.gameObject.name == volumeType);
        foreach (Sound s in mysounds)
        {
            s.volume = value;
            Debug.Log(s.source.gameObject.name);
            s.source.volume = value;


        }
    }
    public void setCanvasActive(bool _active)
    {
        BG.gameObject.SetActive(_active);
    }



}