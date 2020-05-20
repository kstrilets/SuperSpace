using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    //Setting array of asteroids prefabs
    [SerializeField] GameObject[] asteroid;

    Vector2 screenBounds;
    float xPosition;
  
    void Start()
    {
        screenBounds = ScreenBound.bounds;       
        InvokeRepeating("StartAsteroid", 0f, 3f);
    }

   
    void StartAsteroid()
    {
        //getting random asteroid from the array
        int number = Random.Range(0, asteroid.Length);
        Instantiate(asteroid[number]);
        xPosition = Random.Range(screenBounds.x * -4, screenBounds.x * 4);
        asteroid[number].transform.position = new Vector2(xPosition, screenBounds.y + 0.5f);
    }
}
