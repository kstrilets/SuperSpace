using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// is called from EnemyShooting class
public class EnemyBulletRadiantMovement : MonoBehaviour
{

    public void Move(Rigidbody2D rb, Vector3 direction, float speed)
    {
        rb.velocity = direction * speed;
    }


    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
