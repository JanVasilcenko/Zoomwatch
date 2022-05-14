using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityTemplateProjects.UI;

// reference https://www.youtube.com/watch?v=C1gCOoDU29M
public class MixerController : MonoBehaviour
{
  
  public AudioMixer audioMixer;
  
  public TextMeshProUGUI masterText; 
  public TextMeshProUGUI sfxText;
  public TextMeshProUGUI musicText;
  public TextMeshProUGUI sensitivityX;
  public TextMeshProUGUI sensitivityY;

  public MouseSettings mouseSettings; 



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

  public void SetSensitivityX(float value)
  {
    sensitivityX.text = value.ToString("0.0");
    mouseSettings.sensitivityX = value * 100;
  }
  public void SetSensitivityY(float value)
  {
    sensitivityY.text = value.ToString("0.0");
    mouseSettings.sensitivityY = value * 100;

  }
}
