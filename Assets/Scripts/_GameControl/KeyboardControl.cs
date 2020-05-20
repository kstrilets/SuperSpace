using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//simple keyboard control script for testing on Mac/PC
public class KeyboardControl : MonoBehaviour
{
    
    Rigidbody2D rb;
    Vector3 direction;
  

    float keyboardX;
    float keyboardY;

    float speed = 5f;

    [Range(0, .3f)] [SerializeField] float movementSmoothing = .05f;
    Vector2 velocity = Vector2.zero;



    void Start()
    {
        // Caching rigidbody and main camera

        rb = GetComponent<Rigidbody2D>();

        //Setting initial ship coord point
        direction = new Vector3(0, -3, 0);
    }


    // calculating all vectors in Update method
    void Update()
    {
        //getting keyboard input
        keyboardX = Input.GetAxis("Horizontal") * speed;
        keyboardY = Input.GetAxis("Vertical") * speed;

    }

    void FixedUpdate()
    {
        MoveKeyboard();
    }

    // moving based on smoothed velocity
    void MoveKeyboard()
    {
        Vector2 targetVelocity = new Vector2(keyboardX, keyboardY);
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);
    }

    
}
