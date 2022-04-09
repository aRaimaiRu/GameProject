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
    [SerializeField] Sound[] bgmPlayList;
    int index = 0;
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
            setSoundToSource(s);
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
        setSoundToSource(s);
        s.source.Play();
    }

    public void setVolume(string volumeType, float value)
    {

        Sound[] mysounds = Array.FindAll<Sound>(sounds, sound => sound.source.gameObject.name == volumeType);
        foreach (Sound s in volumeType == "bgm" ? bgmPlayList : mysounds)
        {
            s.volume = value;
            s.source.volume = value;
        }

    }
    public void setCanvasActive(bool _active)
    {
        BG.gameObject.SetActive(_active);
    }

    private void Update()
    {
        if (bgmPlayList[index].source.isPlaying == false)
        {
            Sound s = bgmPlayList[index];

            setSoundToSource(s);
            s.source.Play();
            index = (index + 1) % bgmPlayList.Length;
        }
    }
    private void setSoundToSource(Sound s)
    {
        s.source.clip = s.clip;
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
    }





}