using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Spawning diff player's bonuses
public class PlayerHealthSpawn : MonoBehaviour
{
    [SerializeField] GameObject playerShieldBonus;
    [SerializeField] GameObject playerNitroBonus;
    [SerializeField] GameObject playerAddLifeBonus;

    Vector2 screenBounds;
    float xPositionRandom;

    void Start()
    {
        //Getting screenbounds
        screenBounds = ScreenBound.bounds;

        //Start spawning
        StartCoroutine(Spawner(playerShieldBonus, 5f, 20f));
        StartCoroutine(Spawner(playerNitroBonus, 7f, 30f));
        StartCoroutine(Spawner(playerAddLifeBonus, 40f, 80f * (LevelGenerator.level + 1)));
    }

    IEnumerator Spawner (GameObject bonusToSpawn, float startDelay, float nextDelay)
    {
        WaitForSeconds instDelay = new WaitForSeconds(nextDelay);
        yield return new WaitForSeconds(startDelay); 

        while (gameObject != null)
        {
            //Setting random position along the X axis
            xPositionRandom = Random.Range(-screenBounds.x + 0.5f, screenBounds.x - 0.5f);
            Instantiate(bonusToSpawn);
            bonusToSpawn.transform.position = new Vector2(xPositionRandom, screenBounds.y + 1f);
            yield return instDelay;
        }
    }
}
