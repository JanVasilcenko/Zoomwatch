using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class MixerController : MonoBehaviour
{
  public AudioMixer audioMixer;
  public TextMeshProUGUI masterText; 
  public TextMeshProUGUI sfxText; 

  public TextMeshProUGUI musicText; 

  public void SetMasterVolume(float value)
  {
    masterText.text = value.ToString("0.0");
    audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
  }
  public void SetSFXVolume(float value)
  {
    sfxText.text = value.ToString("0.0");
    audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
  }
  public void SetMusicVolume(float value)
  {
    musicText.text = value.ToString("0.0");

    audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
  }
}
