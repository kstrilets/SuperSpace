using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Moving of the Asteroids: they are moving diagonally to random targets across the screen
public class AsteroidMoving : MonoBehaviour
{
    //Asteroid rigibody, speed and rotation speed
    Rigidbody2D rb;
    float speed;
    float rotationSpeed;

    Vector2 target;
    Vector2 screenBounds;

    //Setting asteroid's speed, rot speed and initial position
    void Start()
    {
        screenBounds = ScreenBound.bounds;
        rb = GetComponent<Rigidbody2D>();

        if (transform.position.x >= 0)
        {
            target.x = Random.Range(screenBounds.x * -2, 0);
        }
        else
        {
            target.x = Random.Range(0, screenBounds.x * 2);
        }

        target.y = Random.Range(-screenBounds.y - 1, -screenBounds.y * 2);
        rotationSpeed = Random.Range(-10f, 10f);
        speed = Random.Range(1f, 2f);
    }

    //moving and rotating asteroid
    void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position,
                                                 target, speed * Time.fixedDeltaTime);
        rb.AddTorque(rotationSpeed * Time.fixedDeltaTime);


        //Checking if asteroid is out of lower screen bounds, then destroy
        if (transform.position.x > screenBounds.x + 1 ||
            transform.position.x < -screenBounds.x - 1 ||
            transform.position.y < -screenBounds.y - 1)
        {
            Destroy(gameObject);
        }
    }
}
