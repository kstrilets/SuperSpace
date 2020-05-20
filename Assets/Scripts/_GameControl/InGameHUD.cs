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
    //[SerializeField] Text lowFuelWarning;
    [SerializeField] Text invincibleText;

    [SerializeField] Image playerHealthBar;  //ie shield
    [SerializeField] Image playerFireCharge;
   // [SerializeField] Image playerFireCharge2;
  //  [SerializeField] Image playerFireCharge3;

    StringBuilder sb_newAreaText = new StringBuilder("AREA ", 8);

    string levelNumber;
   // Text component_lowFuelWarning;
    Text component_isInvincible;
    bool lowFuel;
    bool flashCoroutineStarted;
    
    public static int score;
    public static int hiscore;
    public static int lifes;
    public static int ammoHUD;

    bool isCenterBullet;
    //public static int kills;
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
       // component_lowFuelWarning = lowFuelWarning.GetComponent<Text>();
       // component_lowFuelWarning.enabled = false;
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

        for (int i = 131; i >= 1; i-=2)
        {
            newAreaText.fontSize = i;
            yield return frame;
        }
        sb_newAreaText.Clear();
        sb_newAreaText.Append("AREA ");

        //updating upper HUD level value
        leveNumberText.text = "AREA";
        levelNumberTextNumber.text = (LevelGenerator.level).ToString();
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
           // playerFireCharge2.fillAmount = 0;
           // playerFireCharge3.fillAmount = 0;
        }

        else
        {
            playerFireCharge.fillAmount = (float)PlayerState.playerFireBonus / PlayerState.playerFireBonusMax3;

        }

        //else if (PlayerState.playerFireBonus <= PlayerState.playerFireBonusMax1) // player firebonus is less then Max1
        //{
        //    playerFireCharge.fillAmount = (float)PlayerState.playerFireBonus / PlayerState.playerFireBonusMax1;
        //    // this check is for decreasing values of playerFireBonus
        //    if (PlayerState.playerFireBonus < PlayerState.playerFireBonusMax1)
        //        playerFireCharge2.fillAmount = 0;
        //    // If player has Charge bonus up to second level
        //    //  start displaying second charge bonus (minus first charge bonus cap)

        //} else if (PlayerState.playerFireBonus > PlayerState.playerFireBonusMax1  //player fire bonus is between Max1 and Max2
        //             && PlayerState.playerFireBonus <= PlayerState.playerFireBonusMax2)
        //    {
        //    playerFireCharge2.fillAmount = (float)(PlayerState.playerFireBonus - PlayerState.playerFireBonusMax1)
        //                                    / (PlayerState.playerFireBonusMax2 - PlayerState.playerFireBonusMax1);


        //    // this check is for decreesing values of playerFireBonus
        //    if (PlayerState.playerFireBonus < PlayerState.playerFireBonusMax2)
        //        playerFireCharge3.fillAmount = 0;
        //}
        //else if (PlayerState.playerFireBonus > PlayerState.playerFireBonusMax2      //player fire bonus is between Max2 and Max3
        //        && PlayerState.playerFireBonus <= PlayerState.playerFireBonusMax3)
        //{
        //    playerFireCharge3.fillAmount = (float)(PlayerState.playerFireBonus - PlayerState.playerFireBonusMax2 )/
        //                                        (PlayerState.playerFireBonusMax3 - PlayerState.playerFireBonusMax2);
        //}
    }


    // Filling Player's Fuel Bar in real-time --- warning obsolete!

    void PlayerFuelBar()
    {

    }


    // //flashing warning method
    //void Flash()
    //{
    //    //checking if coroutine not started then start it
    //    //to prevent multiple coroutines 
    //    if (!flashCoroutineStarted)
    //    {
    //        StartCoroutine(LowFuelWarningFlash());
    //    }
    //}

    //Low fuel warning coroutine
    //IEnumerator LowFuelWarningFlash()
    //{
    //    WaitForSeconds delay = new WaitForSeconds(0.5f);

    //    while (lowFuel)
    //    {
    //        component_lowFuelWarning.enabled = !component_lowFuelWarning.enabled;
    //        yield return delay;           
    //    }
    //    yield return null;

    //}


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
