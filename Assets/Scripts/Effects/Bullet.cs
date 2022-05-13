using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    [Range(5, 100)]
    [Tooltip("Bullet destroys after time")]
    public float destroyAfter;
    [Tooltip("If enabled the bullet destroys on impact")]
    public bool destroyOnImpact = false;
    [Tooltip("Minimum time after impact that the bullet is destroyed")]
    public float minDestroyTime;
    [Tooltip("Maximum time after impact that the bullet is destroyed")]
    public float maxDestroyTime;
    public int damage;

    private bool isEnemyBullet;

    public GameObject bloodImpactPrefab;
    public GameObject otherImpactPrefab;

    void Start() {
        StartCoroutine(DestroyAfter());
    }

    public void IsEnemyBullet() {
        isEnemyBullet = true;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.GetComponent<Bullet>() != null) {
            return;
        }

        if (isEnemyBullet && collision.gameObject.CompareTag(Tags.player)) {
            if (collision.gameObject.GetComponentInParent<HealthSystemPlayer>() != null) {
                collision.gameObject.GetComponentInParent<HealthSystemPlayer>().TakeDamage(damage);
            }
            
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("whatIsEnemies")) {

            if (collision.gameObject.GetComponentInParent<HealthSystem>() != null) {
                collision.gameObject.GetComponentInParent<HealthSystem>().TakeDamage(damage);
            }

            Instantiate(bloodImpactPrefab, transform.position,
                Quaternion.LookRotation(collision.contacts [0].normal));
            
            Destroy(gameObject);
        }
        else {
            Instantiate(otherImpactPrefab, transform.position,
                Quaternion.LookRotation(collision.contacts [0].normal));
        }
    }

    private IEnumerator DestroyAfter() {
        yield return new WaitForSeconds(destroyAfter);
        Destroy(this.gameObject);
    }
}
