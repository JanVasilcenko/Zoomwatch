using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour {
    [SerializeField]
    private float delayUntilDestroy;

    public AudioSource audioSource;
    public AudioClip audioClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource.isPlaying && audioSource)
        {
               audioSource.PlayOneShot(audioClip);
        }
     
        Invoke("Destroy", delayUntilDestroy);
    }

    private void Destroy() {
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Zombie"))
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
}
