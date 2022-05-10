using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int diamondValue;
    private bool keyValue;
    private int neededValue;

    public TextMeshProUGUI diamondText;
    public TextMeshProUGUI restDiamondText;
    
    public TextMeshProUGUI keyText;
    public TextMeshProUGUI restKeyText;
    
    public TextMeshProUGUI keyEndText;
    public TextMeshProUGUI keyValueText;

    public AudioClip diamondSound;

    private AudioSource audioSource;

    void Start()
    {
        diamondValue = 6;
        keyValue = false;
        diamondText.alpha = 0;
        keyText.enabled = false;
        restKeyText.enabled = false;
        keyEndText.enabled = false;
        restDiamondText.alpha = 0;
        neededValue = 7;
        keyEndText.enabled = false;

        audioSource = GetComponent<AudioSource>();
        InvokeRepeating("FadeIn",6.1f, 0f);
    }

    public void incrementDiamonds(){
        diamondValue++;
        diamondText.text = diamondValue.ToString();
        
        audioSource.PlayOneShot(diamondSound);
        if (diamondValue == neededValue)
        {
            keyText.enabled = true;
            restKeyText.enabled = true;
            StartCoroutine(waiter());
            DestroyText();
        }
    }

    public void setKey(){
        keyValueText.text = 1.ToString();

        keyValue = true;
    }

    public bool getKey()
    {
        return keyValue;
    }

    public int getDiamonds(){
        return diamondValue;
    }

    void DestroyText()
    {
        Destroy(diamondText);
        Destroy(restDiamondText);
    }

    void FadeIn()
    {
       
        diamondText.alpha = 1;
        restDiamondText.alpha = 1;
        
    }

    IEnumerator waiter()
    {
        keyEndText.enabled = true;
        yield return new WaitForSecondsRealtime(6);
        keyEndText.CrossFadeAlpha(0,1f,false);
        yield return new WaitForSecondsRealtime(1);

        keyEndText.enabled = false;
        Destroy(keyEndText);
    }

}
