using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour {
    [SerializeField]
    private float delayUntilDestroy;

    public AudioSource audioSource;
    public AudioClip audioClip;

    void Start() {
        if (!audioSource.isPlaying)
        {
               audioSource.PlayOneShot(audioClip);
        }
     
        Invoke("Destroy", delayUntilDestroy);
    }

    private void Destroy() {
        Destroy(this.gameObject);
    }

}
