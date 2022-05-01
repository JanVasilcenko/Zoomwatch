using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
      //bullet 
    public GameObject bullet;
    public GameObject bullet1;
    public GameObject bullet2;
    public GameObject bullet3;
    public GameObject bullet4;

    //ammo
    public int bulletAmmo2, bulletAmmo3, bulletAmmo4;

    //bullet force
    public float shootForce, upwardForce;

    //Gun stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;

    int bulletsLeft, bulletsShot;

    //Recoil
    public Rigidbody playerRb;
    public float recoilForce;

    //bools
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera camera;
    public Transform attackPoint;

    //Graphics
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;

    //bug fixing :D
    public bool allowInvoke = true;

    private void Awake()
    {
        bulletAmmo2 = 0;
        bulletAmmo3 = 0;
        bulletAmmo4 = 0;
        changeGun1();
        //make sure magazine is full
        bulletsLeft = magazineSize;
        readyToShoot = true; 
    }

    private void Update()
    {
        MyInput();

        //Set ammo display, if it exists :D
        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
    }
    private void MyInput()
    {

        if(Input.GetButtonDown("Gun1"))
        {
            changeGun1();
        }

        if(Input.GetButtonDown("Gun2"))
        {
            changeGun2();
        }

        if(Input.GetButtonDown("Gun3"))
        {
            changeGun3();
        }

        if(Input.GetButtonDown("Gun4"))
        {
            changeGun4();
        }
        
        
        //Check if allowed to hold down button and take corresponding input
        if (allowButtonHold) shooting = Input.GetButton("Shoot") || (Input.GetAxis("Shoot") != 0);
        else shooting = Input.GetButtonDown("Shoot") || (Input.GetAxis("Shoot") != 0);

        //Reloading 
        //if (Input.GetButtonDown("Reload") && bulletsLeft < magazineSize && !reloading) Reload();
        //Reload automatically when trying to shoot without ammo
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0 && bullet == bullet1) Reload();

        //Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            //Set bullets shot to 0
            bulletsShot = 0;

            Shoot();
        }
    }

    private void Shoot()
    {
        
        //streialnie 
        readyToShoot = false;

        //Find the exact hit position using a raycast
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //Just a ray through the middle of your current view
        RaycastHit hit;

        //check if ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75); //Just a point far away from the player

        //Calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        //Calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate new direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0); //Just add spread to last direction

        //Instantiate bullet/projectile
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity); //store instantiated bullet in currentBullet
        //Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;

        //Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(camera.transform.up * upwardForce, ForceMode.Impulse);

        //Instantiate muzzle flash, if you have one
        if (muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot++;

        //Invoke resetShot function (if not already invoked), with your timeBetweenShooting
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;

            //Add recoil to player (should only be called once)
            playerRb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);
        }

        //if more than one bulletsPerTap make sure to repeat shoot function
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }
    private void ResetShot()
    {
        //Allow shooting and invoking again
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime); //Invoke ReloadFinished function with your reloadTime as delay
    }
    private void ReloadFinished()
    {
        //Fill magazine
        bulletsLeft = magazineSize;
        reloading = false;
    }

    private void changeGun1() 
    {
        this.bullet = bullet1;
        this.shootForce = 50;
        this.timeBetweenShooting = 0.5f;
        this.spread = 0;
        this.magazineSize = 1;
        this.reloadTime = 0;
        this.allowButtonHold = false;
        this.recoilForce = 0;
        this.bulletsPerTap = 1;
    }

    private void changeGun2() 
    {
        this.bullet = bullet2;
        this.shootForce = 100;
        this.timeBetweenShooting = 0.5f;
        this.spread = 0;
        this.magazineSize = bulletAmmo4;
        this.reloadTime = 0;
        this.allowButtonHold = false;
        this.recoilForce = 1;
        this.bulletsPerTap = 5;
    }

    private void changeGun3() 
    {
        this.bullet = bullet3;
        this.shootForce = 50;
        this.timeBetweenShooting = 0.1f;
        this.spread = 0.0f;
        this.magazineSize = bulletAmmo3;
        this.reloadTime = 0;
        this.allowButtonHold = true;
        this.recoilForce = 0;
        this.bulletsPerTap = 1;
    }

    private void changeGun4() 
    {
        this.bullet = bullet4;
        this.shootForce = 50;
        this.timeBetweenShooting = 1;
        this.spread = 0;
        this.magazineSize = bulletAmmo4;
        this.reloadTime = 0;
        this.allowButtonHold = true;
        this.recoilForce = 0;
        this.bulletsPerTap = 1;
    }
}
