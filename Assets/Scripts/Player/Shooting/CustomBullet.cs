using UnityEngine;

public class CustomBullet : MonoBehaviour {
    //Assignables
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask whatIsEnemies;

    //Stats
    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;

    //Damage
    public int explosionDamage;
    public float explosionRange;
    public float explosionForce;

    //Lifetime
    public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnTouch = true;

    int collisions;
    PhysicMaterial physics_mat;

    private void Start() {
        Setup();
    }

    private void Update() {
        //When to explode:
        if (collisions > maxCollisions)
            Explode();

        //Count down lifetime
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0)
            Explode();
    }

    private void Explode() {
        //Instantiate explosion
        if (explosion != null)
            Instantiate(explosion, transform.position, Quaternion.identity);

        //Check for enemies 
        Collider [] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
        for (int i = 0; i < enemies.Length; i++) {
            //Get component of enemy and call Take Damage
            
            if (enemies [i].GetComponent<HealthSystem>() != null) {
                enemies [i].GetComponent<HealthSystem>().TakeDamage(explosionDamage);
            }

            //Add explosion force (if enemy has a rigidbody)
            if (enemies [i].GetComponent<Rigidbody>())
                enemies [i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange);
        }

        //Add a little delay, just to make sure everything works fine
        Invoke("Delay", 0.01f);
    }
    private void Delay() {
        explosionDamage = 0;
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision) {
        //Don't count collisions with other bullets
        if (collision.collider.CompareTag(Tags.bullet))
            return;

        //Count up collisions
        collisions++;

        //Explode if bullet hits an enemy directly and explodeOnTouch is activated
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("whatIsEnemies") && explodeOnTouch)
            Explode();
    }

    private void Setup() {
        //Create a new Physic material
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;
        //Assign material to collider
        GetComponent<SphereCollider>().material = physics_mat;

        //Set gravity
        rb.useGravity = useGravity;
    }
}
