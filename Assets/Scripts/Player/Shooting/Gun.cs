using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Gun : MonoBehaviour {
    //bullet 
    public GameObject bullet;
    public GameObject bullet1;
    public GameObject bullet2;
    public GameObject bullet3;
    public GameObject bullet4;
    
    //ammo
    public int bulletAmmo1, bulletAmmo2, bulletAmmo3, bulletAmmo4;

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


    //images
    public Image bullet1Image;
    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;
    public Sprite sprite4;


    //bug fixing :D
    public bool allowInvoke = true;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip regularGunSound;
    public AudioClip shoutgunSound;
    public AudioClip laserSound;
    public AudioClip granadeSound;
    public AudioClip switchSound;

    private void Awake() {
        bulletAmmo1 = 9999;
        bulletAmmo2 = 0;
        bulletAmmo3 = 0;
        bulletAmmo4 = 0;
        changeGun1();
        //make sure magazine is full
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update() {
        MyInput();
    }

    private void MyInput() {

        if (!PauseMenu.isPaused) {
            if (Input.GetButtonDown("Gun1")) {
                changeImg1();
                changeGun1();
            }

            if (Input.GetButtonDown("Gun2")) {

                changeImg2();

                changeGun2();
            }

            if (Input.GetButtonDown("Gun3")) {

                changeImg3();

                changeGun3();
            }

            if (Input.GetButtonDown("Gun4")) {
                changeImg4();

                changeGun4();

            }
        }

        //Check if allowed to hold down button and take corresponding input
        if (allowButtonHold)
            shooting = Input.GetButton("Shoot") || (Input.GetAxis("Shoot") != 0);
        else
            shooting = Input.GetButtonDown("Shoot") || (Input.GetAxis("Shoot") != 0);
        
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0 && bullet == bullet1)
            Reload();

        //Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0 && !PauseMenu.isPaused) {
            //Set bullets shot to 0
            bulletsShot = 0;

            Shoot();
        }
    }

    public void UpdateAmmunitionText(int bulletAmmo) {
        //Set ammo display, if it exists :D
        if (ammunitionDisplay != null) {
            if (bulletAmmo < 0) {
                ammunitionDisplay.SetText("∞");
            }
            else {
                ammunitionDisplay.SetText((bulletAmmo / bulletsPerTap).ToString());
            }
        }
    }

    private void Shoot() {

        if (bullet == bullet1) {
            if (regularGunSound != null)
                UpdateAmmunitionText(-1);
            audioSource.PlayOneShot(regularGunSound);
        }
        else if (bullet == bullet2) {
            if (bulletAmmo2 > 0) {
                audioSource.PlayOneShot(shoutgunSound);
                bulletAmmo2--;
                UpdateAmmunitionText(bulletAmmo2);
            }
            else
                return;
        }
        else if (bullet == bullet3) {
            if (bulletAmmo3 > 0) {
                audioSource.PlayOneShot(laserSound);
                bulletAmmo3--;
                UpdateAmmunitionText(bulletAmmo3);
            }
            else
                return;
        }
        else {
            if (bulletAmmo4 > 0) {
                audioSource.PlayOneShot(granadeSound);
                bulletAmmo4--;
                UpdateAmmunitionText(bulletAmmo4);
            }
            else
                return;
        }

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
        if (allowInvoke) {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;

            //Add recoil to player (should only be called once)
            playerRb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);
        }

        //if more than one bulletsPerTap make sure to repeat shoot function
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }
    private void ResetShot() {
        //Allow shooting and invoking again
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload() {
        reloading = true;
        Invoke("ReloadFinished", reloadTime); //Invoke ReloadFinished function with your reloadTime as delay
    }
    private void ReloadFinished() {
        //Fill magazine
        bulletsLeft = magazineSize;
        reloading = false;
    }

    private void changeGun1() {
        this.bullet = bullet1;
        this.shootForce = 50;
        this.timeBetweenShooting = 0.5f;
        this.spread = 0;
        this.magazineSize = 999;
        this.reloadTime = 0;
        this.allowButtonHold = false;
        this.recoilForce = 0;
        this.bulletsPerTap = 1;
        UpdateAmmunitionText(-1);
    }

    private void changeGun2() {
        this.bullet = bullet2;
        this.shootForce = 100;
        this.timeBetweenShooting = 0.5f;
        this.spread = 0;
        this.magazineSize = bulletAmmo4;
        this.reloadTime = 0;
        this.allowButtonHold = false;
        this.recoilForce = 1;
        this.bulletsPerTap = 5;
        UpdateAmmunitionText(bulletAmmo2);
    }

    private void changeGun3() {
        this.bullet = bullet3;
        this.shootForce = 50;
        this.timeBetweenShooting = 0.1f;
        this.spread = 0.0f;
        this.magazineSize = bulletAmmo3;
        this.reloadTime = 0;
        this.allowButtonHold = true;
        this.recoilForce = 0;
        this.bulletsPerTap = 1;
        UpdateAmmunitionText(bulletAmmo3);
    }

    private void changeGun4() {
        this.bullet = bullet4;
        this.shootForce = 50;
        this.timeBetweenShooting = 1;
        this.spread = 0;
        this.magazineSize = bulletAmmo4;
        this.reloadTime = 0;
        this.allowButtonHold = true;
        this.recoilForce = 0;
        this.bulletsPerTap = 1;
        UpdateAmmunitionText(bulletAmmo4);
    }

    private void changeImg1() {
        if (bullet != bullet1) {
            audioSource.PlayOneShot(switchSound);
        }

        if (bullet1)

            bullet1Image.sprite = sprite1;
    }
    private void changeImg2() {

        if (bullet != bullet2) {
            audioSource.PlayOneShot(switchSound);
        }

        if (bullet1Image)
            bullet1Image.sprite = sprite2;

    }
    private void changeImg3() {
        if (bullet != bullet3) {
            audioSource.PlayOneShot(switchSound);
        }

        if (bullet1Image)
            bullet1Image.sprite = sprite3;
    }

    private void changeImg4() {
        if (bullet != bullet4) {
            audioSource.PlayOneShot(switchSound);
        }

        if (bullet1Image) {
            bullet1Image.sprite = sprite4;
        }

    }
}
