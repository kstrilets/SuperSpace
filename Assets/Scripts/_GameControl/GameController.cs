using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Getting parts of UI, spawner to spawn shield (health or other) objects,
    // player prefab etc.
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject lostMenuUI;
    [SerializeField] GameObject spawnerObject;  // -- collectibles spawner
    [SerializeField] GameObject winMenuUI;
    [SerializeField] GameObject quitConfirmationUI;
    [SerializeField] GameObject audioManager;
    [SerializeField] GameObject soundMenuUI;
    [SerializeField] GameObject creditsMenuUI;
    [SerializeField] GameObject quitCurrentGameConfirmationUI;
   
    [SerializeField] GameObject playerPrefab; //-- player prefab gameobject

    [SerializeField] GameObject levelGeneratorPrefab;  //-- gameobject on which level generator sits

    [SerializeField] Toggle vibration;  //-- phone vibration

    Vector3 playerStartPosition;
    GameObject playerInst;

    public static bool isInvincible; // -- bool to set player is undestructible after being dead and reborn
    public static bool isVibrate = true;

    bool isPauseMenu; //-- bool to check if the menu page (sound options) is loaded
                      //from the pause menu or the main menu

    void OnEnable()
    {
        EventManager.StartListening("STARTNEWLIFE", GameIni);
        EventManager.StartListening("GAMEOVER", GameLost);
        EventManager.StartListening("GAMEPAUSED", Pause);
        EventManager.StartListening("GAMEWIN", GameWin);
    }

    void OnDisable()
    {
        EventManager.StopListening("STARTNEWLIFE", GameIni);
        EventManager.StopListening("GAMEOVER", GameLost);
        EventManager.StopListening("GAMEPAUSED", Pause);
        EventManager.StopListening("GAMEWIN", GameWin);
    }


    private void Awake()
    {   
        Instantiate(audioManager); //-- instattiating AudioManager
    }

    void Start()
    {
        // Game entrance point. Stopping game time, activating main menu
        //setting some player stats

        Time.timeScale = 0f;
        mainMenuUI.SetActive(true);
        playerStartPosition.x = 0;
        playerStartPosition.y = -3;
        playerStartPosition.z = 0;
      
        AudioManager.instance.Play("menu_music");
    }

    //Starting new game from MENU (after pressing "Start New Game" button)
    public void StartNewGame()
    {
        mainMenuUI.SetActive(false);
      
        //clearing all gameobjects, player and spawner prefabs
        DestroyAllGameObjects();
        DestroyPlayerObject();
        DestroySpawner();

        // instantiating player, setting that player has full health and no charge 
        // to deside which helpers to spawn (shield or charge)

        playerInst = Instantiate(playerPrefab, playerStartPosition, Quaternion.identity);
       

        // setting player stats for the new game in PlayerState class
        PlayerState.NewGame();

        // Starting LevelGenerator Prefab
        Instantiate(levelGeneratorPrefab);

        // Starting extra help-objects (ie shield bonus) 
        Instantiate(spawnerObject);

        // Starting game time 
        Time.timeScale = 1f;

        // Stopping menu music and starting game music
        AudioManager.instance.Stop("menu_music");
        AudioManager.instance.Play("bg_music");
    }

    //Turning on Main Menu
    public void LoadMainMenu()
    {
        lostMenuUI.SetActive(false);
        mainMenuUI.SetActive(true);
        winMenuUI.SetActive(false);
        quitConfirmationUI.SetActive(false);
        quitCurrentGameConfirmationUI.SetActive(false);
        soundMenuUI.SetActive(false);
        creditsMenuUI.SetActive(false);

        isPauseMenu = false;

        Time.timeScale = 0f;

        AudioManager.instance.Stop("bg_music");
        AudioManager.instance.Play("menu_music");
    }


    // Listener to PlayerState.LifeLost
    // if lifes > 0, starts coroutine to blink players sprite
    // via turning it off and on and setting player invincibility to true
    // and after 2 sec to false to prevent player loosing health from collisions
    public void GameIni()
    {
        EventManager.TriggerEvent("STARTNEWLIFEAFTERKILL");
        SpriteRenderer sp = playerInst.GetComponent<SpriteRenderer>();
        StartCoroutine(PlayerReborn(sp));
    }

    // Coroutine for GameIni method
    IEnumerator PlayerReborn(SpriteRenderer playerSprite)
    {
        WaitForSeconds delay = new WaitForSeconds(0.1f);

        if (playerInst != null)
        {
            isInvincible = true;
            EventManager.TriggerEvent("ISINVINCIBLE"); //turning HUD message on


            // player ship is blinking for 2 sec
            for (int i = 0; i < 20; i++)
            {
                playerSprite.enabled = false;
                yield return delay;
                playerSprite.enabled = true;
                yield return delay;
            }
            isInvincible = false;
            EventManager.TriggerEvent("ISINVINCIBLE"); //turning HUD message off

        }
    }

  
   
    // Setting game on pause and turning on Pause menu
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        soundMenuUI.SetActive(false);
        quitCurrentGameConfirmationUI.SetActive(false);

        isPauseMenu = true;

        AudioManager.instance.Stop("bg_music");
        AudioManager.instance.Play("menu_music");

        Time.timeScale = 0f;   
    }


    // quit current game confirmation
    public void QuitCurrentGameConfirmation()
    {
        pauseMenuUI.SetActive(false);
        quitCurrentGameConfirmationUI.SetActive(true);
    }


    // quit application confirmation
    public void QuitConfirmation()
    {
        mainMenuUI.SetActive(false);
        quitConfirmationUI.SetActive(true);
    }

    //Resuming current game and turning off pause menu
    public void Resume()
    {
        pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;
        AudioManager.instance.Stop("menu_music");
        AudioManager.instance.Play("bg_music");

    }


    //application exit
    public void Quit()
    {
        Application.Quit();
    }


    //checking if sound menu was called from Pause Menu or Main Menu
    public void WhereSoundMenu()
    {
        if (isPauseMenu)
        {
            Pause();
        } else
        {
            LoadMainMenu();
        }
    }


    //Loading Sounds Control Menu
    public void LoadSoundMenu()
    {
        pauseMenuUI.SetActive(false);
        mainMenuUI.SetActive(false);
        soundMenuUI.SetActive(true);
    }

    // controlling vibraiotn from the sound menu
    public void VibrationOnOff()
    {
        if (vibration.isOn)
        {
            isVibrate = true;
        }

        else
        {
            isVibrate = false;
        }

    }


    //Loading credits Menu
    public void LoadCreditsMenu()
    {
        mainMenuUI.SetActive(false);
        creditsMenuUI.SetActive(true);
    }

    // Starting Game lost Menu after 2 sec after player destroed
    public void GameLost()
    {
        Destroy(playerInst);
        DestroyAllGameObjects();
        DestroySpawner();
        PlayerState.playerFireBonus = 0;
        EventManager.TriggerEvent("PLAYERCHARGEBAR");

        Invoke("LoadGameLost", 2f);
        
    }
    void LoadGameLost()
    {
        lostMenuUI.SetActive(true);

        AudioManager.instance.Stop("bg_music");
        AudioManager.instance.Play("game_lost");
    }

    //Starting Game Win Menu after 2 sec last area finished and final boss killed
    public void GameWin()
    {
        Invoke("LoadGameWin", 2f);
        DestroyAllGameObjects();
    }

    void LoadGameWin()
    {
        winMenuUI.SetActive(true);

        AudioManager.instance.Stop("bg_music");
        AudioManager.instance.Play("game_win");

        DestroyAllGameObjects();
        DestroySpawner();
    }


    // Destroing ALL the game objects: getting objects with corresponding tags into an arrays
    // then destroing every element of each array 
    public void DestroyAllGameObjects()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy_1");
        foreach (GameObject o in enemies)
        {
            Destroy(o);
        }

        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Player_bullet");
        foreach (GameObject o in bullets)
        {
            Destroy(o);
        }

        GameObject[] enemyBullets = GameObject.FindGameObjectsWithTag("Enemy_bullet");
        foreach (GameObject o in enemyBullets)
        {
            Destroy(o);
        }

        GameObject[] ammoBoxes = GameObject.FindGameObjectsWithTag("Player_health");
        foreach (GameObject o in ammoBoxes)
        {
            Destroy(o);
        }

        GameObject[] fuelBoxes = GameObject.FindGameObjectsWithTag("Add_fuel");
        foreach (GameObject o in fuelBoxes)
        {
            Destroy(o);
        }

        GameObject[] lifeBoxes = GameObject.FindGameObjectsWithTag("Add_life");
        foreach (GameObject o in lifeBoxes)
        {
            Destroy(o);
        }

        GameObject[] fireBoxes = GameObject.FindGameObjectsWithTag("Fire_bonus");
        foreach (GameObject o in fireBoxes)
        {
            Destroy(o);
        }

        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        foreach (GameObject o in asteroids)
        {
            Destroy(o);
        }

        GameObject[] land = GameObject.FindGameObjectsWithTag("Land");
        foreach (GameObject o in land)
        {
            Destroy(o);
        }

        GameObject[] levelGenerator = GameObject.FindGameObjectsWithTag("LevelGenerator01");
        foreach (GameObject o in levelGenerator)
        {
            Destroy(o);
        }

    }

    // Destroing all Spawners
    public void DestroySpawner()
    {
        GameObject[] spawner = GameObject.FindGameObjectsWithTag("Spawner");
        foreach (GameObject o in spawner)
        {
            Destroy(o);
        }
    }

    // Destroing all Players gameobjects
    public void DestroyPlayerObject()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Destroy(player);
    }
}