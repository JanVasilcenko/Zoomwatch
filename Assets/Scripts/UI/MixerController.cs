using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Audio;

// reference https://www.youtube.com/watch?v=C1gCOoDU29M
public class MixerController : MonoBehaviour {

    public AudioMixer audioMixer;

    public TextMeshProUGUI masterText;
    public TextMeshProUGUI sfxText;
    public TextMeshProUGUI musicText;
    public TextMeshProUGUI sensitivityX;
    public TextMeshProUGUI sensitivityY;
    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider musicSlider;
    public Slider sensitivityXSlider;
    public Slider sensitivityYSlider;
    public bool isGameMenu;

    private void Start() {
        masterSlider.value = GetKeyIfExistsMusic("MasterVolume");
        sfxSlider.value = GetKeyIfExistsMusic("SFXVolume");
        musicSlider.value = GetKeyIfExistsMusic("MusicVolume");
        if (!isGameMenu) {
            sensitivityXSlider.value = GetKeyIfExistsSensitivity("SensitivityX") / 100;
            sensitivityYSlider.value = GetKeyIfExistsSensitivity("SensitivityY") / 100;
        }
    }

    private float GetKeyIfExistsMusic(string key) {
        if (PlayerPrefs.HasKey(key)) {
            return PlayerPrefs.GetFloat(key);
        }
        return 1f;
    }

    private float GetKeyIfExistsSensitivity(string key) {
        if (PlayerPrefs.HasKey(key)) {
            return PlayerPrefs.GetFloat(key);
        }
        return 100f;
    }

    public void SetMasterVolume(float value) {
        masterText.text = value.ToString("0.0");
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MasterVolume", value);
    }
    public void SetSFXVolume(float value) {
        sfxText.text = value.ToString("0.0");
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }
    public void SetMusicVolume(float value) {
        musicText.text = value.ToString("0.0");
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSensitivityX(float value) {
        sensitivityX.text = value.ToString("0.0");
        PlayerPrefs.SetFloat("SensitivityX", value * 100);
    }
    public void SetSensitivityY(float value) {
        sensitivityY.text = value.ToString("0.0");
        PlayerPrefs.SetFloat("SensitivityY", value * 100);
    }
}
