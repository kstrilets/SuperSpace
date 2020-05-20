using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//moving different types of enemies in different ways
public class EnemyMoving : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D col;
    float yBound;

    //Simple moving down with random speed possibility
    [SerializeField] bool movingDown;
    [SerializeField] float speed;
    [SerializeField] bool isVariable;
    [SerializeField] float movingDownMin;
    [SerializeField] float movingDownMax;

    //moving down in sinuse wave manner with random magnitude posiibility
    [SerializeField] bool movingWave;
    [SerializeField] float speedForward;
    [SerializeField] float speedSpline;
    [SerializeField] float minMagnitude;
    [SerializeField] float maxMagnitude;
    [SerializeField] float frequency;
    Vector3 position;
    float magnitude;
    float fr;

    //moving down and then up with a short stop (Go-Stop-Go)
    [SerializeField] bool movingGSG;
    [SerializeField] float speedGSG;
    [SerializeField] float startSpeedGSG;
    [SerializeField] float delayGSG;
    [SerializeField] float positionGSG;
    [SerializeField] bool isChanging;
    [SerializeField] string spriteNames;
    SpriteRenderer sr;
    Sprite sprites;
    Animator animator;
    bool firstEntryGSG;

    //moving with rotation
    [SerializeField] bool rotating;
    [SerializeField] float rotationSpeed;
    float randomZAngle;

    //following 
    [SerializeField] bool movingFollow;
    [SerializeField] float speedFollow;
    [SerializeField] float stopDistance;
    Transform target;

    //Caching components, setting random vars
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        sprites = Resources.Load<Sprite>("enemy_calamary_run_away");

        yBound = ScreenBound.bounds.y;

        //if gameobject has collider2D - turn it off while gameobject is outside
        //visibility to prevent making damage to the outside screen objects
        if (gameObject.GetComponent<Collider2D>() != null)
        {
            col = gameObject.GetComponent<Collider2D>();
            col.enabled = false;
        }

        //simple downward moving, adding force to move the object
        if (movingDown)
        {
            if (isVariable)
            {
                speed = Random.Range(movingDownMin, movingDownMax);
            }
            rb.AddForce(-transform.up * speed);
        }

        //Wave like movement, setting randome wave components
        if (movingWave)
        {
            position = transform.position;
            magnitude = Random.Range(minMagnitude, maxMagnitude);
            fr = Random.Range(1, 5);
        }

        //adding force to the object if Go-Stop-Go movement selected
        if (movingGSG)
        {
            rb.AddForce(-transform.up * startSpeedGSG);
        }

        //if object is moving with rotation - setting random angles
        if (rotating)
        {
            randomZAngle = Random.Range(0, 360);
            gameObject.transform.Rotate(0, 0, randomZAngle);
        }

        //if the Player object exists setting it as a target to follow
        //if not - adding simple downward force
        if (movingFollow)
        {
            if (GameObject.FindGameObjectsWithTag("Player").Length > 0)
            {
                target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            } else
            {
                rb.AddForce(-transform.up * speed);
            }
        }
    }

    // on became visible turning obj collider back so it can get damaged
    void OnBecameVisible()
    {
        if (gameObject.GetComponent<Collider2D>() != null)
        {
            col = gameObject.GetComponent<Collider2D>();
            col.enabled = true;
        }
    }

    void FixedUpdate()
    {
        //in Go-Stop-Go movement cheking if gameobject is in position where it have to stop
        //and if so - starting delay coroutine
        if (movingGSG)
        {
            if (gameObject.transform.position.y < positionGSG) //float.Epsilon
            {
                StartCoroutine(Wait());
            }

            //because calamari enters and exits the same upper screen side
            //we are checking number of crossing upper screen side
            //and destroing them after the secon one
            if (gameObject.transform.position.y < yBound)
                firstEntryGSG = true;

            if (gameObject.transform.position.y > yBound + 1
                && firstEntryGSG)
            {
                Destroy(gameObject);
            }
        }

        //simple wave movement
        if (movingWave)
        {
            MovingWave();
        }

        //rotating while moving down
        if (rotating)
        {
            gameObject.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }

        //following player
        if (movingFollow)
        {
            if (target != null)
            {
                if (Vector2.Distance(transform.position, target.position) > stopDistance)
                {
                    transform.position = Vector2.MoveTowards(transform.position,
                        target.position, speed * Time.deltaTime);
                }
            }
        }


        //Checking Screen Bounds, if the object outside the lower bound - destroy it
        if (transform.position.y < -ScreenBound.bounds.y - 1)
        {
            Destroy(gameObject);
        }
    }


    //coroutine for the short stop in Go-Stop-Go movement
    //and for sprite changing after the stop
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(delayGSG);
        if (isChanging)
        {
            animator.enabled = false;
            sr.sprite = sprites;
        }   
        rb.AddForce(transform.up * speedGSG);
    }

    //waving move method
    void MovingWave()
    {
        position += -transform.up * Time.deltaTime * speedForward;
        gameObject.transform.position = position +
                             (-transform.right *
                             Mathf.Cos(transform.position.y * speedSpline + Time.time * frequency) * magnitude);
    }

}
