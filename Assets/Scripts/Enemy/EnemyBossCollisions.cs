using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Enemy Bosses Collisions

public class EnemyBossCollisions : MonoBehaviour
{

    // Explosion Prefab for current Boss
    [SerializeField] GameObject enemyExplosionPrefab;

    // Checking if this is final Boss for current level
    // (if boss is final after its death we start generating new level)
    [SerializeField] bool isFinalBoss;

    //Boss healthpoints
    [SerializeField] float enemyStartHealth;

    //Checking if boss is static or moving for the right instantiating of the
    // explosion prefab

    [SerializeField] bool isMoving;

    // Boss health bar variable
    [SerializeField] Image healthBar;

    //float enemyDamageTaken;
    float enemyHealth;


    // Boss is starting with full health
    void Start()
    {
        enemyHealth = enemyStartHealth;
    }


    // In case of collision with Player buller or Player itself
    // Boss's health is reducing
    
    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player_small_bullet"))
        {

            enemyHealth -= PlayerState.smallBulletDamage;
            EnemyHealthLost();
        }

        if (collision.CompareTag("Player"))
        {
            enemyHealth -= PlayerState.smallBulletDamage;
            EnemyHealthLost();
        }


    }


    // Checking is Boss is dead (ie health <= 0) or just damaged
    // If damaged - set proper amount of health left on its HealthBar
    void EnemyHealthLost()
    {

        //enemyHealth -= enemyDamageTaken;

        if (enemyHealth < 0)
            enemyHealth = 0;

        healthBar.fillAmount = (enemyHealth / enemyStartHealth);

        if (enemyHealth <= 0)
        {
            EnemyDead();
        }
    }


    // If Boss is dead
    
    void EnemyDead()

    {
        // Starting its explosion
        GameObject explosion = Instantiate(enemyExplosionPrefab, transform.position, Quaternion.identity);

        // If Boss is final boss for this level triggering event to load next level
        if (isFinalBoss)
        {
            

            EventManager.TriggerEvent("LOADNEXTLEVEL");
        }

        // Triggering event for score adding
        //EventManager.TriggerEvent("SCOREADDED");

        // If Boss was located on the land sprite (ie moving with it)
        // destroy it but do not destroy explosion prefab
        if (isMoving)
        {
            Destroy(gameObject);
        }

        // If Boss was located in space, destroy it and then destroy
        // explosion prefab after 1 sec
        else
        {
            Destroy(gameObject);
            Destroy(explosion, 1f);
        }
    }
}
