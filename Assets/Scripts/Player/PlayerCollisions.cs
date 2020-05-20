using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Collisions of the player's ship with objects and enemies
public class PlayerCollisions : MonoBehaviour
{
   //Vibrate method for the phone vibration in collisions
    void Vibrate ()
    {
        if (GameController.isVibrate)
            Handheld.Vibrate();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //health lost from colliding enemy or enemy ammo

        //coll with enemy ship
        if (collision.CompareTag("Enemy_1"))
        {
            PlayerState.playerHealthChange = -4;
            EventManager.TriggerEvent("PLAYERHEALTHLOST");
          
            Vibrate();
        }

        //coll with enemy ammo
        if (collision.CompareTag("Enemy_bullet"))

        {
            PlayerState.playerHealthChange = -2; 
            EventManager.TriggerEvent("PLAYERHEALTHLOST");

            Destroy(collision.gameObject);

            Vibrate();
        }

        //coll with Asteroid
        if (collision.CompareTag("Asteroid"))
        {
            Destroy(collision.gameObject);
            PlayerState.playerHealthChange = -2;
            EventManager.TriggerEvent("PLAYERHEALTHLOST");
            Vibrate();
        }

        //coll with player's fire bonus
        if (collision.CompareTag("Fire_bonus"))
        {
            AudioManager.instance.Play("charge_pickup");

            EventManager.TriggerEvent("FIREBONUSCHANGE");
            Destroy(collision.gameObject);
        }

        //coll with player's health(shield) bonus

        if (collision.CompareTag("Player_health"))
        {
            AudioManager.instance.Play("shield_pickup");

            PlayerState.playerHealthChange = 1;
            EventManager.TriggerEvent("HEALTHADDED");
            Destroy(collision.gameObject);
        }

        //coll with player's fireRate bonus
        if (collision.CompareTag("Add_fuel"))
        {
            AudioManager.instance.Play("fuel_pickup");

            Destroy(collision.gameObject);
            PlayerShooting.fireRateBonus = true;
        }

        //coll with player's life bonus
        if (collision.CompareTag("Add_life"))
        {
            AudioManager.instance.Play("shield_pickup");

            Destroy(collision.gameObject);
            PlayerState.playerLifes++;
            EventManager.TriggerEvent("PLAYERADDLIFE");
        } 
    }
}
