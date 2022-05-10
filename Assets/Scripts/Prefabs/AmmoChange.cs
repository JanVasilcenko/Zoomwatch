using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoChange : MonoBehaviour
{
    public Gun gun;

    public GameObject bullet1;
    public GameObject bullet2;
    public GameObject bullet3;
    public GameObject bullet4;

    void Start()
    {
        gun = GetComponent<Gun>();

        getGun1();
    }

    void Update()
    {
        if(Input.GetButtonDown("Gun1"))
        {
            getGun1();
        }

        if(Input.GetButtonDown("Gun2"))
        {
            getGun2();
        }

        if(Input.GetButtonDown("Gun3"))
        {
            getGun3();
        }

        if(Input.GetButtonDown("Gun4"))
        {
            getGun4();
        }
    }

    private void getGun1(){
        gun.bullet = bullet1;
        gun.timeBetweenShooting = 0.1f;
    }
    private void getGun2(){
        gun.bullet = bullet2;
    }
    private void getGun3(){
        gun.bullet = bullet3;
    }
    private void getGun4(){
        gun.bullet = bullet4;
    }
}
