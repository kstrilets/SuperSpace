using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Checking collisions on enemies and instantiating player's bonuses from enemys' death
public class EnemyCollisions : MonoBehaviour
{
    //enemy explosion animation
    [SerializeField] GameObject enemyExplosionPrefab;

    //player's bonuses
    [SerializeField] GameObject fireBonus;
    [SerializeField] GameObject healthBonus;
    [SerializeField] GameObject fuelBonus;


    //enemy start health
    [SerializeField] float enemyStartHealth;

    //This check is for cannons whos explosion is keeps mooving after cannon destruction
    [SerializeField] bool isMoving;

    //internal enemy health var
    float enemyHealth;

    //checks if bonus already instantiated 
    bool isBonus;

    //internal enemy health is the start enemy health
    void Start()
    {
        enemyHealth = enemyStartHealth;
    }

    //cheking collisions
    void OnTriggerEnter2D(Collider2D collision)
    {
        //if collision is with players bullet decreasing enemy health with bullet damage
        // and if enemy health is less than 0 - enemy is dead
        if (collision.CompareTag("Player_small_bullet"))
        {
            enemyHealth -= PlayerState.smallBulletDamage;
            if (enemyHealth <= 0)
            {
                EnemyDead();
            }

        }

        //if collision with player - instant enemy death
        if (collision.CompareTag("Player"))
        {
            EnemyDead();
        }
    }

    //if enemy is dead
    void EnemyDead()

    {

        //play death sound
        AudioManager.instance.Play("enemy_death");

        //inst explosion
        GameObject explosion = Instantiate(enemyExplosionPrefab, transform.position, Quaternion.identity);

        //if this is cannon - destroy cannon prefab
        if (isMoving)
        {
            Destroy(gameObject);
        }

        //if this is smth else - destroy prefab and explosion
        else
        {
            Destroy(gameObject);
            Destroy(explosion, 1f);
        }


        //randomly inst shooting fuel(nitro) bonus
        int randFuel = Random.Range(0, 20);

        if (randFuel == 3)
        {
            Instantiate(fuelBonus, transform.position, Quaternion.identity);
            isBonus = true;
        }

        //randomly inst shield or fire bonus if there was no fuel bonus
        int rand = Random.Range(0, 12);

        if (rand == 2 && isBonus == false)
        {
            Instantiate(healthBonus, transform.position, Quaternion.identity);

        } else if ((rand == 7 || rand == 4) && isBonus == false)

        {
            Instantiate(fireBonus, transform.position, Quaternion.identity);
        } 
    }  
}
