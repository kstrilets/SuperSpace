using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannonBullet : MonoBehaviour
{
    // Cannon bullet movement script
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float force;

    //Adding force for movement
    void Start()
    {
        rb.AddForce(transform.right * force, ForceMode2D.Impulse);
    }

    //Destroing bullet on invisible
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
