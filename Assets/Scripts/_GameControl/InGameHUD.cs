using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

// In-game HUD is responsible for displaying game stats in real time

public class InGameHUD : MonoBehaviour
{

    [SerializeField] Text lifesText;
    [SerializeField] Text leveNumberText;
    [SerializeField] Text levelNumberTextNumber;
    [SerializeField] Text newAreaText;  //entering new area text
    [SerializeField] Text invincibleText;

    [SerializeField] Image playerHealthBar;  //ie shield
    [SerializeField] Image playerFireCharge;
  

    StringBuilder sb_newAreaText = new StringBuilder("AREA ", 8);

    string levelNumber;
    Text component_isInvincible;
    bool lowFuel;
    bool flashCoroutineStarted;
    
    public static int score;
    public static int hiscore;
    public static int lifes;
    public static int ammoHUD;

    bool isCenterBullet;
    float smoothTime = 10f;
    float t;


    private void OnEnable()
    {
        EventManager.StartListening("PLAYERHEALTHBAR", PlayerHealthBar);
        EventManager.StartListening("PLAYERCHARGEBAR", PlayerChargeBar);
        EventManager.StartListening("PLAYERFUELCHANGE", PlayerFuelBar);
        EventManager.StartListening("NEWAREATEXT", NewAreaText);
        EventManager.StartListening("PLAYERADDLIFE", PlayerAddLife);
        EventManager.StartListening("ISINVINCIBLE", IsInvincible);
    }
    private void OnDisable()
    {
        EventManager.StopListening("PLAYERHEALTHBAR", PlayerHealthBar);
        EventManager.StopListening("PLAYERCHARGEBAR", PlayerChargeBar);
        EventManager.StopListening("PLAYERFUELCHANGE", PlayerFuelBar);
        EventManager.StopListening("NEWAREATEXT", NewAreaText);
        EventManager.StopListening("PLAYERADDLIFE", PlayerAddLife);
        EventManager.StopListening("ISINVINCIBLE", IsInvincible);
    }

    private void Start()
    {
        // Getting initial score, number of lifes and Area (level) number and storing
        // them at local vars

        lifesText.text = PlayerState.playerLifes.ToString();
        leveNumberText.text = "AREA";
        levelNumberTextNumber.text = (LevelGenerator.level + 1).ToString();
        PlayerState.playerFireBonus = 0;
        component_isInvincible = invincibleText.GetComponent<Text>();
        component_isInvincible.enabled = false;
        playerHealthBar.fillAmount = 0.5f;
    }


    // Generating New Area blow up letters
    void NewAreaText ()
    {
        //creating string with the number of the level
        levelNumber = (LevelGenerator.level + 1).ToString();

        //
        newAreaText.fontSize = 1;
        newAreaText.text = "AREA ";

        //appending string with level number to string builder object
        sb_newAreaText.Append(levelNumber);
        newAreaText.text = sb_newAreaText.ToString();

       

        //starting zoom-in zoom-out effect
        StartCoroutine(NewAreaTextCoroutine());
    }


    // zoom-in zoom-out text effect
    IEnumerator NewAreaTextCoroutine()
    {
        WaitForSeconds delay_1 = new WaitForSeconds(1f);
        WaitForSeconds delay_2 = new WaitForSeconds(3f);

        WaitForEndOfFrame frame = new WaitForEndOfFrame();

        if (LevelGenerator.level != 0)
            yield return delay_2;

        for (int i = 1; i <= 131; i+=2)
        {
            newAreaText.fontSize = i;
            yield return frame;
        }

        yield return delay_1;
        //updating upper HUD level value
        leveNumberText.text = "AREA";
        levelNumberTextNumber.text = (LevelGenerator.level + 1).ToString();

        for (int i = 131; i >= 1; i-=2)
        {
            newAreaText.fontSize = i;
            yield return frame;
        }
        sb_newAreaText.Clear();
        sb_newAreaText.Append("AREA ");

       
        yield return null;
    }

    // Filling player shield (or health) bar in real time
    public void PlayerHealthBar()
    {
        playerHealthBar.fillAmount = PlayerState.playerHealth / PlayerState.playerStartHealth;
       
    }

    // Filling player Charge Bar in real time
    void PlayerChargeBar()
    {
        // Checking if player has any Charge Bonus
        // if none - setting bars to zero
        if (PlayerState.playerFireBonus <= 0)
        {
            playerFireCharge.fillAmount = 0;
          
        }

        else
        {
            playerFireCharge.fillAmount = (float)PlayerState.playerFireBonus / PlayerState.playerFireBonusMax3;

        }

      
    }


   void PlayerFuelBar()
    {

    }

    //Displaying the number of player's lifes
    void PlayerAddLife()
    {
        lifesText.text = PlayerState.playerLifes.ToString();
    }

    void IsInvincible()
    {
          if (GameController.isInvincible)
        {
            component_isInvincible.enabled = true;
        } else
        {
            component_isInvincible.enabled = false;
        }
    }

}
