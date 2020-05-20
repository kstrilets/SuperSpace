using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Players bullet movement and collisions 
public class SmallBullet : MonoBehaviour
{
    [SerializeField] GameObject playerBulletExplosionPrefab;
    Rigidbody2D rb;

    float bulletForce = 15;

    // Moving bullet with force
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();  
        rb.velocity = transform.up * bulletForce;
    }

    //Destroing outside the screen
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    //Bullet collisions
    void OnTriggerEnter2D(Collider2D collision)
    {
        // if bullet is colliding when player has Full Charge Bonus it is not destroing in collision
        if (PlayerShooting.fullChargedBonus && collision.CompareTag("Enemy_1") || collision.CompareTag("Asteroid"))
        {
            GameObject explosion = Instantiate(playerBulletExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 0.5f);
        }

        // if bullet is colliding when player does not has Full Charge Bonus it is destroing in collision
        if (collision.CompareTag("Enemy_1") || collision.CompareTag("Asteroid"))
        {
            GameObject explosion = Instantiate(playerBulletExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(explosion, 0.5f);
        }

    }
}
