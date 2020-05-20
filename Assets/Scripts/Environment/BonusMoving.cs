using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Moving player's bonuses from top to down
public class BonusMoving : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Rigidbody2D rb;

    void Start()
    {
        Destroy(gameObject, 15f);
        rb.AddForce(-transform.up * speed, ForceMode2D.Impulse);
    }
}
