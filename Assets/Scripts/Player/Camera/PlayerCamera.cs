using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCamera : MonoBehaviour {
    public float sensitivityX, sensitivityY;
    public Transform orientation;
    public Transform cameraHolder;

    private float rotationX, rotationY;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update() {
        if (!PauseMenu.isPaused)
        {
              //get mouse input
                    float mouseX = Input.GetAxis("Mouse X") * Time.fixedDeltaTime * sensitivityX;
                    float mouseY = Input.GetAxis("Mouse Y") * Time.fixedDeltaTime * sensitivityY;
            
                    rotationY += mouseX;
                    rotationX -= mouseY;
                    rotationX = Mathf.Clamp(rotationX, -90f, 90f);
            
                    //rotate cam and orientation
                    cameraHolder.rotation = Quaternion.Euler(rotationX, rotationY, 0);
                    orientation.rotation = Quaternion.Euler(0, rotationY, 0);
        }
      
    }

    public void DoFov(float endValue)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }

    public void DoTilt(float zTilt)
    {
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
    }
}
