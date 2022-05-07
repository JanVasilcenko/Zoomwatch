using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUps : MonoBehaviour
{
    public bool ammoPickUp;
    public bool healthPickUp;

    public int healthAmount, ammoAmount2, ammoAmount3, ammoAmount4;

    public AudioClip healthSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void addAmmo(){
        Gun gun = GetComponent<Gun>();
        gun.bulletAmmo2 += ammoAmount2;
        gun.bulletAmmo3 += ammoAmount3;
        gun.bulletAmmo4 += ammoAmount4;
        Debug.Log("PickUP ammo");
    }

    private void addHealth(){
        AudioSource.PlayClipAtPoint(healthSound, gameObject.transform.position);
        //todo
        Debug.Log("PickUP health");
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            //set visibility of object
            if(ammoPickUp)
                addAmmo();
            if(healthPickUp)
                addHealth();
        }
    }
}
