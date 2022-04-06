using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour {
    [SerializeField]
    private float delayUntilDestroy;

    void Start() {
        Invoke("Destroy", delayUntilDestroy);
    }

    private void Destroy() {
        Destroy(this.gameObject);
    }

}
