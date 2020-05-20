using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Moving purple Boss
public class EnemyBossPurpleMoving : MonoBehaviour
{

    // Getting start Y-axis location
    [SerializeField] float startYPosition;

    //Getting time to stay at every new position
    [SerializeField] float timeToMove;

    Vector3 target;

    float randomXPosition;
    float speed = 0.8f;
    float startTime;


    void Start()
    {

        //Moving Boss to its start position 
        gameObject.transform.position = new Vector2(0, startYPosition);

        //Setting first random X-coord to move Purple Boss along X-Axis
        randomXPosition = Random.Range(-1.8f, 1.8f);

        //Setting target for the first Purple Boss movement
        target = new Vector3(randomXPosition, startYPosition, 0);
      
    }

    private void Update()
    {
        // Moving Purple Boss from its position to target position
        gameObject.transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);


        // If Purple Boss gets to the target position starting timer
        if (transform.position == target)
        {
            startTime += Time.deltaTime;


            // If timer comes to the end calculating new X-position and new
            // target vector to move Purple Boss, setting timer to zero
            if (startTime > timeToMove)
            {
                randomXPosition = Random.Range(-1.8f, 1.8f);
                if (Mathf.Abs(transform.position.x - randomXPosition) > 0.5f)
                {
                    target = new Vector3(randomXPosition, startYPosition, 0);
                    startTime = 0;

                }
                else
                {
                    randomXPosition = Random.Range(-1.8f, 1.8f);

                }

            }
            
            
        }

    }




}
