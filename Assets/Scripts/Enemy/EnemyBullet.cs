using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Moving enemy bullet
public class EnemyBullet : MonoBehaviour
{
    // Getting bullets' rigidbody and speed
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed;

    void Start()
    {
        // Moving bullet from up to down with speed
        rb.velocity = -transform.up * speed;

    }

    //if bullet collides with asteroid will be destroed
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Asteroid"))
        {
            GameObject explosion = Instantiate(gameObject, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(explosion, 0.5f);
        }
    }

    // Destroing after become invisible
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}

