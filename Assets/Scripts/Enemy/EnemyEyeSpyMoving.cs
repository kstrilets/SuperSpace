using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//moving eye spy from left side of the screen to the right side or opposite
public class EnemyEyeSpyMoving : MonoBehaviour
{
    float speed;
    Vector2 target;
    float xBound;
    Rigidbody2D rb;

    void Start()
    {
        xBound = ScreenBound.bounds.x;
        rb = GetComponent<Rigidbody2D>();
        target.x = -transform.position.x;
        target.y = transform.position.y;
        speed = Random.Range(1f, 3f);
        Destroy(gameObject, 8f);
    }

    void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime);
    }
}
