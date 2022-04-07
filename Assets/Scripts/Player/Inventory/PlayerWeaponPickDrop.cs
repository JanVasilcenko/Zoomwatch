using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponPickDrop : MonoBehaviour
{
    public GameObject camera;
    float maxPickupDistance = 5f;
    GameObject currentItem;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("e")) 
            PickUpWeapon();
        if(Input.GetKeyDown("q"))
            DropWeapon();
    }

    private void PickUpWeapon()
    {
        RaycastHit hit;
        if(Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, maxPickupDistance))
        {
            if(hit.transform.tag == "weapon")
            {
                // if(currentItem.transform.parent != null && currentItem.transform.tag == "weapon")
                //     DropWeapon();

                currentItem = hit.transform.gameObject;

                foreach (var c in hit.transform.GetComponentsInChildren<Collider>())
                {
                    if(c != null)
                        c.enabled = false;
                }
                foreach (var s in hit.transform.GetComponentsInChildren<WeaponScript>())
                {
                    if(s != null)
                        s.enabled = false;
                }

                currentItem.transform.parent = transform;
                currentItem.transform.localPosition = Vector3.zero;
                currentItem.transform.localEulerAngles = Vector3.zero;
            }
        }
    }

    private void DropWeapon()
    {
        currentItem.transform.parent = null;
        foreach (var c in currentItem.transform.GetComponentsInChildren<Collider>())
        {
            if(c != null)
                c.enabled = true;
        }
        foreach (var s in currentItem.transform.GetComponentsInChildren<WeaponScript>())
        {
            if(s != null)
                s.enabled = true;
        }
        
        RaycastHit hitDown;
        Physics.Raycast(transform.position, -Vector3.up, out hitDown);

        currentItem.transform.position = hitDown.point + new Vector3(transform.forward.x, 0.5f, transform.forward.z);
    }
}
