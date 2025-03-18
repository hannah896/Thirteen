using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider masterSlider, bgmSlider, sfxSlider;
    [SerializeField] private GameObject settingMenu;
        
    private void Start()
    {
        AudioManager audio = AudioManager.instance;

        masterSlider.value = audio.MasterValue;
        bgmSlider.value = audio.BGMValue;
        sfxSlider.value = audio.SFXValue;
    }

    public void SetMasterVolume()
    {
        float volume = masterSlider.value;
        myMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void SetBGMVolume()
    {
        float volume = bgmSlider.value;
        myMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void Active()
    {
        AudioManager.instance.PlaySFX("Button");
        settingMenu.SetActive(!settingMenu.activeSelf);
    }
}
