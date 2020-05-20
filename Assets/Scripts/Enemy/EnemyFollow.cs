using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// enemy is following the Player or going to some screen coord if player is dead
public class EnemyFollow : MonoBehaviour
{
    //setting speed of the object which is following somewhere
    [SerializeField] float speed;

    //if it is following player set the stopping distance from player
    [SerializeField] bool isFollowingPlayer;
    [SerializeField] float stopDistance;

    //if it is following to the screen coord, setting screen coord
    [SerializeField] bool isFollowingScreenCoord;
    [SerializeField] float targetPositionX;
    [SerializeField] float targetPositionY;

    Transform target;
    Vector2 targetPosition;

    //if there is object with tag "Player" - set it as target
    void Start()
    {
        if (GameObject.FindGameObjectsWithTag("Player").Length > 0 )
        {
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }

        targetPosition = new Vector2(targetPositionX, targetPositionY);
    }


    void Update()
    {
        if (isFollowingPlayer && !isFollowingScreenCoord)
        {
            FollowingPlayer();
        }

        if (isFollowingScreenCoord && !isFollowingPlayer)
        {
            FollowingScreenCoord();
        }
        
    }


    //if target exists (ie Player) - follow him and stop at stop distance
    void FollowingPlayer()
    {
        if (target != null)
        {
            if (Vector2.Distance(transform.position, target.position) > stopDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position,
                                                        target.position,
                                                        speed* Time.deltaTime);
            }
        }
    }

    void FollowingScreenCoord()
    {
        transform.position = Vector2.MoveTowards(transform.position,
                                                        targetPosition,
                                                        speed * Time.deltaTime);
    }
}
