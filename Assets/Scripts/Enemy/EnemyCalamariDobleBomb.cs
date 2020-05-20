using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Calamari bomb - slowly floating and rotating
//with random rotation speed
public class EnemyCalamariDobleBomb : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] bool isRotating;
    [SerializeField] float rotationSpeedMin;
    [SerializeField] float rotationSpeedMax;


    float randomX;
    float randomY;
    float rotationSpeed;

    private void Start()
    {
        randomX = Random.Range(3f, -3f);
        randomY = Random.Range(0f, -3f);
        rotationSpeed = Random.Range(rotationSpeedMin, rotationSpeedMax);
        rb.velocity = (-transform.up + new Vector3(randomX, randomY, 0)) * speed;
    }

    private void Update()
    {
        if (isRotating)
            rb.AddTorque(rotationSpeed * Time.deltaTime);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
