using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Touch control for player sprite
public class TouchControl : MonoBehaviour
{
    Vector3 touchPosition;
    Rigidbody2D rb;
    Vector3 direction;
    Camera cam;
    Vector3 startTouch;
    float moveSpeed = 0.1f;

    float keyboardX;
    float keyboardY;

    [Range(0, .3f)] [SerializeField] float m_MovementSmoothing = .05f;
    Vector3 m_Velocity = Vector3.zero;



    void Start()
    {
        // Caching rigidbody and main camera

        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;

        //Setting initial ship coord point
        direction = new Vector3(0, -3, 0);
    }


    // calculating all vectors in Update method
    void Update()
    {
        // Counting number of touches
        // If touch happens and player ship fuel > 0

        //if (Input.touchCount > 0 && PlayerState.playerFuel > 0f) --->// this is variant if movement depends on fuel
        keyboardX = Input.GetAxisRaw("Horizontal");
        keyboardY = Input.GetAxisRaw("Vertical");

      

        if (Input.touchCount > 0)

            {
                // storing touch and transferring screen coordinates to world coordinates
                Touch touch = Input.GetTouch(0);
            touchPosition = cam.ScreenToWorldPoint(new Vector2(touch.position.x,
                                                               touch.position.y));

            // switch on touch phases

            switch (touch.phase)
            {
                case TouchPhase.Began:

                    //Calculating start touch vector
                    startTouch.x = touchPosition.x - transform.position.x;
                    startTouch.y = touchPosition.y - transform.position.y;

                    break;

                case TouchPhase.Moved:

                    // Calculating touch direction
                    direction = touchPosition - startTouch;
                    break;

                case TouchPhase.Ended:

                    // Stop moving when touch phase finished
                    break;
            }   
        }
    }

    // calling Move method in FixedUpdate phase
    private void FixedUpdate()
    {
        if (Input.touchCount != 0)
        {
            Move();
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            MoveKeyboard(keyboardX);

        }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            MoveKeyboard(keyboardY);

        }
    }

    // Moving the rigidbody of the player based on the touch direction calculated in Update
    // Calculating Player Fuel and triggering HUD
    void Move()
    {  
        rb.MovePosition(direction + moveSpeed * direction * Time.fixedDeltaTime);

        //below is the variant if movement depends on fuel

        //PlayerState.playerFuel -= Time.fixedDeltaTime * PlayerShooting.fuelConsumption;
        //EventManager.TriggerEvent("PLAYERFUELCHANGE");
    }

    void MoveKeyboard(float move)
    {
        Vector3 targetVelocity = new Vector2(move * 10f, rb.velocity.y);

        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
    }
   
}
