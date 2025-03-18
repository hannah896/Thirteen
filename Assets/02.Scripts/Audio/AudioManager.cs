using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Audio[] bgmAudios, sfxAudios;
    public AudioMixer myMixer;
    public AudioSource bgmSource, sfxSource;

    public float MasterValue { get; private set; }
    public float BGMValue { get; private set; }
    public float SFXValue {  get; private set; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            PlayMusic("Background");
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "StartScene")
        {
            PlayMusic("StartBackground");
        }
        else if(scene.name == "MainScene")
        {
            PlayMusic("MainBackground");
        }
    }

    public void PlayMusic(string name)
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            MasterValue = PlayerPrefs.GetFloat("MasterVolume");
            myMixer.SetFloat("Master", Mathf.Log10(MasterValue) * 20);
        }
        else
        {
            MasterValue = 0.5f;
        }

        if (PlayerPrefs.HasKey("BGMVolume"))
        {
            BGMValue = PlayerPrefs.GetFloat("BGMVolume");
            myMixer.SetFloat("BGM", Mathf.Log10(BGMValue) * 20);
        }
        else
        {
            BGMValue = 0.5f;
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            SFXValue = PlayerPrefs.GetFloat("SFXVolume");
            myMixer.SetFloat("SFX", Mathf.Log10(PlayerPrefs.GetFloat("SFXValue")) * 20);
        }
        else
        {
            SFXValue = 0.5f;
        }


        Audio s = Array.Find(bgmAudios, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            bgmSource.clip = s.clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Audio s = Array.Find(sfxAudios, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void ToggleMusic()
    {
        bgmSource.mute = !bgmSource.mute;
    }

    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void MusicVolume(float volume)
    {
        bgmSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    private void OnDestroy()
    {       
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
