using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Rotating cannons toward the Player 
public class EnemyRotatingToward : MonoBehaviour
{
    [SerializeField] float rotatingSpeed;

    Transform target;
    Vector3 targetDirection;
    float angle;

    //if Player exists set it as target
    void Start()
    {
        if (GameObject.FindGameObjectsWithTag("Player").Length > 0)
        {
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
    }

    // if target exists rotate cannon turret toward it
    void FixedUpdate()
    {
        if (target != null)
        {
            targetDirection = target.position - transform.position;
            angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}
