using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Spawning small enemy eye %)
public class EnemyEyeSpySpawn : MonoBehaviour
{
    [SerializeField] GameObject enemyEye;

    float yPosition;
    int xPosition;

    void Start()
    {

        //Invoking spy eye every 10 sec
        InvokeRepeating("StartEnemyEye", 10f, 10f);
    }

    //inst spy eye randomly at the right or left side of screen
    void StartEnemyEye()
    {
        Instantiate(enemyEye);
        xPosition = Random.Range(0, 2) * 2 - 1; 
        yPosition = Random.Range(- ScreenBound.bounds.y, ScreenBound.bounds.y);
        enemyEye.transform.position = new Vector2((ScreenBound.bounds.x + 0.5f) * xPosition, yPosition);
    }
}
