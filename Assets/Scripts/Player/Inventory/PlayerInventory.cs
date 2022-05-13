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
    public TextMeshProUGUI keyFoundText;

    public GameObject deathScreen;

    public AudioClip diamondSound;

    private AudioSource audioSource;

    void Start()
    {
        diamondValue = 0;
        keyValue = true;
        diamondText.alpha = 0;
        keyText.enabled = false;
        restKeyText.enabled = false;
        keyEndText.enabled = false;
        keyFoundText.enabled = false;
        restDiamondText.alpha = 0;
        deathScreen.SetActive(false);
        neededValue = 7;

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
            StartCoroutine(waiter(keyEndText));
            DestroyText();
        }
    }

    public void setKey(){
        keyValueText.text = 1.ToString();
        StartCoroutine(waiter(keyFoundText)); 
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

    IEnumerator waiter( TextMeshProUGUI text)
    {
        text.enabled = true;
        yield return new WaitForSecondsRealtime(6);
        text.CrossFadeAlpha(0,1f,false);
        yield return new WaitForSecondsRealtime(1);

        text.enabled = false;
        Destroy(text);
    }

}
