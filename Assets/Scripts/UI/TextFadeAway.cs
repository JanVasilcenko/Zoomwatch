using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFadeAway : MonoBehaviour
{
    public TextMeshProUGUI text;
    void Start()
    {
        InvokeRepeating("FadeText", 4.0f, 0f);
        InvokeRepeating("Destroy", 6.1f, 0f);
    }

    void FadeText()
    {
        text.CrossFadeAlpha(0,2f,false);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
