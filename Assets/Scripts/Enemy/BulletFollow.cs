using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Movement of the bullet that moves into player's direction
//at the moment of shot

public class BulletFollow : MonoBehaviour
{
    [SerializeField] float speed;
    Rigidbody2D rb;
    Vector3 direction;
    Transform target;

    void Start()
    {

        //Cheking if Player is in the game and assigning to variable target
        // player's transform

        if (GameObject.FindGameObjectsWithTag("Player").Length > 0)
        {
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }

        //Cashing bullets RigidBody
        rb = GetComponent<Rigidbody2D>();

        // If Player exists calculating direction to it's transform
        // and adding force to bullet in that direction otherwise destroying
        // the bullet

        if (target != null)
        {
            direction = target.position - transform.position;
            rb.AddForce(direction * speed, ForceMode2D.Impulse);
        }
        else
        {
            Destroy(gameObject);

        }

    }


    // Destroing the bullet that is outside screen

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
