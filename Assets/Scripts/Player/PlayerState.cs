using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Class for keeping and changing player's stats
public class PlayerState : MonoBehaviour
{
    public static int playerLifes;              //number of player's lifes
    public static float playerStartHealth;      //player's start health
    public static float playerHealth;           //player's health during the game

    public static int playerHealthChange;       //amount of player's health added or subtracted during the game
    public static int fireBonusChange;          //changing in player's fire bonus
    public static int playerFireBonus;          //player's fire bonus status

    public static float bulletDamage;           //amount of bullets' damage
    public static float smallBulletDamage;

    public static Vector2 bulletDirection;      //bullets firing directions
    public static Vector2 smallBulletDirection;

    [SerializeField] GameObject playerExplosionPrefab;  //player's explosion
  
    public static int playerFireBonusMax1 = 3;      //cumulative number of player's fire
    public static int playerFireBonusMax2 = 7;      //bonus stages
    public static int playerFireBonusMax3 = 12;

    public static bool isPlayerHealed;              //check if player was healed or they fire bonus
    public static bool isFireBonusChanged;          //was changed

    private void OnEnable()
    {
        EventManager.StartListening("LIFELOST", LifeLost);
        EventManager.StartListening("HEALTHADDED", PlayerHealthChange);
        EventManager.StartListening("STARTNEWLIFEAFTERKILL", StartNewLifeAfterKill);
        EventManager.StartListening("FIREBONUSCHANGE", PlayerFireBonusChange);
        EventManager.StartListening("FIREBONUSDECREMENT", PlayerFireBonusDecrement);
        EventManager.StartListening("PLAYERHEALTHLOST", PlayerHealthChange);
    }

    private void OnDisable()
    {
        EventManager.StopListening("LIFELOST", LifeLost);
        EventManager.StopListening("HEALTHADDED", PlayerHealthChange);
        EventManager.StopListening("STARTNEWLIFEAFTERKILL", StartNewLifeAfterKill);
        EventManager.StopListening("FIREBONUSCHANGE", PlayerFireBonusChange);
        EventManager.StopListening("FIREBONUSDECREMENT", PlayerFireBonusDecrement);
        EventManager.StopListening("PLAYERHEALTHLOST", PlayerHealthChange);
    }

    void Awake()
    {
        playerStartHealth = 12f;            //max possible player's health(shield)
        smallBulletDamage = 1f;             //player's bullet damage
        playerHealth = playerStartHealth/2; //starting game with the half of the shield(health)
        playerFireBonus = 0;                //player's starting fire bonus
        PlayerShooting.fireBonus0 = true;   //starting game with fireBonus0
        playerLifes = 1;                    //starting game with 1 life
    }

    public static void NewGame()            //Starting new game from the main menu
    {
        playerLifes = 1;
        playerFireBonus = 0;
        playerHealth = playerStartHealth/2;
        PlayerShooting.fireBonus0 = true;
        PlayerShooting.fireBonus1 = false;
        PlayerShooting.fireBonus2 = false;
        PlayerShooting.fireBonus3 = false;

                                            //setting HUD to default
        EventManager.TriggerEvent("PLAYERHEALTHBAR");
        EventManager.TriggerEvent("PLAYERCHARGEBAR");
        EventManager.TriggerEvent("PLAYERADDLIFE");
    }

    void StartNewLifeAfterKill()            //starting new life after kill
    {
        playerHealth = playerStartHealth / 2;
        playerFireBonus = 0;
      //  PlayerHealthChange();                //calling this method to make changes in the HUD
        PlayerShooting.fireBonus0 = true;
        PlayerShooting.fireBonus1 = false;
        PlayerShooting.fireBonus2 = false;
        EventManager.TriggerEvent("PLAYERHEALTHBAR");
        EventManager.TriggerEvent("PLAYERCHARGEBAR");
    }


    //changing player's health
    void PlayerHealthChange ()
    {
        //if player is invisible (after life lost for 2 sec)
        //there is no changes in it's health
        if (GameController.isInvincible)        
            playerHealthChange = 0;

        //adding changes to player's health
        //could be positive or negative
        playerHealth += playerHealthChange;

        if (playerHealthChange > 0)
        {
            EventManager.TriggerEvent("PLAYERHEALTHBAR");
        }

        // Setting to zero
        isPlayerHealed = false;
        playerHealthChange = 0;

        // Just in case...
        if (playerHealth < 0)
            playerHealth = 0;

        //clamping player's health to max value wich is player's start health
        if (playerHealth >= playerStartHealth)
        {
            playerHealth = playerStartHealth;
        }

        EventManager.TriggerEvent("PLAYERHEALTHBAR");

        //cheking if player is dead, if so
        //playing sad sound and cutting one life off
        if (playerHealth <= 0)
        {
            AudioManager.instance.Play("player_death");

            LifeLost();
            EventManager.TriggerEvent("PLAYERADDLIFE");
        }
    }

    //player's fire bonus decrement over time
    void PlayerFireBonusDecrement()
    {
        playerFireBonus--;
        if (playerFireBonus <= 0)
            playerFireBonus = 0;

        if (playerFireBonus < playerFireBonusMax3 && playerFireBonus > playerFireBonusMax2)
        {
            PlayerShooting.fireBonus2 = true;
            PlayerShooting.fireBonus1 = true;
            PlayerShooting.fireBonus0 = true;
        }
        if (playerFireBonus <= playerFireBonusMax2 && playerFireBonus > playerFireBonusMax1)
        {
            PlayerShooting.fireBonus2 = false;
            PlayerShooting.fireBonus1 = true;
            PlayerShooting.fireBonus0 = true;
        }
        if (playerFireBonus <= playerFireBonusMax1 )
        {
            PlayerShooting.fireBonus2 = false;
            PlayerShooting.fireBonus1 = false;
            PlayerShooting.fireBonus0 = true;
        }
        EventManager.TriggerEvent("PLAYERCHARGEBAR");
    }

    //player's fire bonus increment while picking bonuses
    void PlayerFireBonusChange()
    {
        playerFireBonus++;
        isFireBonusChanged = true;

        //clamping fire bonus to max
        if (playerFireBonus >= playerFireBonusMax3)
                playerFireBonus = playerFireBonusMax3;

        if (playerFireBonus > playerFireBonusMax1)
            PlayerShooting.fireBonus1 = true;
        if (playerFireBonus > playerFireBonusMax2)
            PlayerShooting.fireBonus2 = true;
        if (playerFireBonus >= playerFireBonusMax3)
            PlayerShooting.fireBonus3 = true;

        EventManager.TriggerEvent("PLAYERCHARGEBAR");
    }

    //if the player lost a life calling explosion method
    //subtracting one life and if the player has more lifes
    //starting new life, if no - triggering game over event
    void LifeLost ()
    {
        PlayerExplosion();

        playerLifes--;
        if (playerLifes < 0)
            playerLifes = 0;

        if (playerLifes > 0)
        {
            EventManager.TriggerEvent("STARTNEWLIFE");
        }
        else if (playerLifes <= 0)
        {
            EventManager.TriggerEvent("GAMEOVER");
        }
    }

    //player's explosion
    void PlayerExplosion()
    {
        GameObject explosion = Instantiate(playerExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, 1f);
    }
}