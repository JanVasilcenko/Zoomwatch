using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponPickDrop : MonoBehaviour
{
    public GameObject camera;
    float maxPickupDistance = 5f;
    GameObject currentItem;
    bool hasWeapon = false;

    void Update()
    {
        if(Input.GetButtonDown("PickUpWeapon"))
            PickUpWeapon();
        if(Input.GetButtonDown("DropWeapon"))
            DropWeapon();
        
        // if(Input.GetKeyDown("1"))
        //     GrabPistol();
        // if(Input.GetKeyDown("2") && currentItem.transform.parent != null)
        //     GrabMainWeapon();
    }

    private void PickUpWeapon()
    {
        RaycastHit hit;
        if(Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, maxPickupDistance))
        {
            if(hit.transform.tag == "weaponUzi" || 
                hit.transform.tag == "weaponRifle" ||
                hit.transform.tag == "weaponKalashnikov")
            {
                Debug.Log(currentItem);
                if(currentItem != null)
                    DropWeapon();


                currentItem = hit.transform.gameObject;
                hasWeapon = true;

                // foreach(var c in hit.transform.GetComponentsInChildren<Collider>())
                // {
                //     if(c != null)
                //         c.enabled = false;
                // }
                // foreach(var s in hit.transform.GetComponentsInChildren<WeaponScript>())
                // {
                //     if(s != null)
                //         s.enabled = false;
                // }

                currentItem.transform.parent = transform;
                currentItem.transform.localPosition = Vector3.zero;
                currentItem.transform.localEulerAngles = Vector3.zero;
            }
        }
    }

    private void DropWeapon()
    {
        currentItem.transform.parent = null;

        // foreach(var c in currentItem.transform.GetComponentsInChildren<Collider>())
        // {
        //     if(c != null)
        //         c.enabled = true;
        // }
        // foreach(var s in currentItem.transform.GetComponentsInChildren<WeaponScript>())
        // {
        //     if(s != null)
        //         s.enabled = true;
        // }

        RaycastHit hitDown;
        Physics.Raycast(transform.position, -Vector3.up, out hitDown);

        currentItem.transform.position = hitDown.point + new Vector3(transform.forward.x, 0.5f, transform.forward.z);

        currentItem = null;
        hasWeapon = false;
    }
}
