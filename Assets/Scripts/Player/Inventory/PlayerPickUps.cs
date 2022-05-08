using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUps : MonoBehaviour
{
    public bool ammoPickUp;
    public bool healthPickUp;

    public int healthAmount, ammoAmount2, ammoAmount3, ammoAmount4;

    public AudioClip healthSound;

    public Gun gun;
    public HealthSystem healthSystem;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void addAmmo(){
        Debug.Log(gun);
        gun.bulletAmmo2 += ammoAmount2;
        gun.bulletAmmo3 += ammoAmount3;
        gun.bulletAmmo4 += ammoAmount4;
        Debug.Log("PickUP ammo end");
    }

    private void addHealth(){
        AudioSource.PlayClipAtPoint(healthSound, gameObject.transform.position);
        healthSystem.Heal(healthAmount);
        Debug.Log("PickUP health");
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            if(ammoPickUp)
                addAmmo();
            if(healthPickUp)
                addHealth();

            //set visibility of object
            gameObject.SetActive(false);
        }
    }
}
