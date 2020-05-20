using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Generating game levels based on array of .png grafic files
// filled with appropriate pixels which colors corresponds to game
// objects. Size of this file could be 8 to 256 px with working area
// 6 px wide (not 8 px!) or 8 to 64 px depends on how big level you want to create
// Unused pixels (which dont has any objects) should be transparent

public class LevelGenerator : MonoBehaviour
{

    // Array of level maps
    public Texture2D[] maps;

    // Assigning colors to prefab game objects
	public ColorToPrefab[] colorMappings;

    // We generate map in chunks to prevent game overloading
    // with yet unused objects and to keep proper speed of moving objects
    // so they will not take over each other too much
    // ie here we are using 8x8 px chunk of the whole map
    // so we should keep placing all objects corresponding to 8x8 px grid in 8x256 px map
    // Map size divided by chunk size HAS to be INT!  
    [SerializeField] int mapChunk = 8;

    Texture2D currentMap;
    public static int level;



    int zPrev;
    float positionShift;
    float xBound;
    float yBound;

    int k;
    int levelNumbers;

    bool levelGenerationFinished = true;
    bool levelBossDead;
    bool generateNextLevel;

    bool _crStarted;

    void OnEnable()
    {
        EventManager.StartListening("LOADNEXTLEVEL", LevelBossIsDead);
        
    }

    void OnDisable()
    {
        EventManager.StopListening("LOADNEXTLEVEL", LevelBossIsDead);
       
    }


    private void Start()
    {
        //Cashing screen bounds
        xBound = ScreenBound.bounds.x;
        yBound = ScreenBound.bounds.y;

        //Getting number of levels 
        levelNumbers = maps.Length;

        //Setting first level
        level = 0;

        //Level generation coroutine is not started
        _crStarted = false;


        //Setting level boss is dead to true to start first level generation 
        levelBossDead = true;

       // EventManager.TriggerEvent("NEWAREATEXT");
    }

    void Update()
    {
       
        //Cheking if it is not the last level, if generation of the previous level
        // is finished, and if previous level boss is dead
        if (level < levelNumbers
            && levelGenerationFinished == true
            && levelBossDead == true
            && _crStarted == false)
        {
           
                //Starting level generator coroutine 
                StartCoroutine(GenerateLevel(level));
                level++;
        }

        // Checking if it was the last level and calling game win UI part
        else if (level > levelNumbers)
        {
            EventManager.TriggerEvent("GAMEWIN");
        }    
    }

    // Level generating coroutine
    IEnumerator GenerateLevel(int _level)
    {


        _crStarted = true;
        // Getting the current map from maps array
        currentMap = maps[_level];

        // Calculating number of chunks based on chunk size and map (image) size in px
        int chunkNumbers = currentMap.height / mapChunk;

        // Calculating map width
        int mapWidth = currentMap.width;

        // Setting triggers to prevent level genetator to stop
        levelBossDead = false;
        levelGenerationFinished = false;


        // time before new map chunk generated
        // valid for 8 pixel chunk
        // hard coded and should be calculated experimentally
        WaitForSeconds delay = new WaitForSeconds(12f);


        EventManager.TriggerEvent("NEWAREATEXT");

        // main loop for level generation
        // looping through map chunks
        for (k = 0; k < chunkNumbers; k++)
        {

            //looping inside chunk (height and width)
            for (int y = 0; y < mapChunk; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {

                    // Calling method for each pixel to get its colors
                    GenerateTile(x, y);
                }
            }

            //if the last chunk, set level generation to finished
            if (k == chunkNumbers - 1)
            {
                levelGenerationFinished = true;
            }

            
            yield return delay;

            
        }

        _crStarted = false;
    }

    void LevelBossIsDead()
    {
        if (level < levelNumbers)
        {

            levelBossDead = true;
            generateNextLevel = true;
        }
        else
        {
            levelBossDead = true;

        }
    }

   
   

    // Getting the color of the pixel and generating prefab based on it
    void GenerateTile(int x, int y)
        {
            // saving pixel color to variable
            Color pixelColor = currentMap.GetPixel(x, y + mapChunk * k);

            // if pixel is transparent no prefab will be generated
            if (pixelColor.a == 0)
            {
                return;
            }

            // Checking the array of color corresponding prefabs
            // if any matching - saving the position of pixel in Vector2 variable
            foreach (ColorToPrefab colorMapping in colorMappings)
            {
                if (colorMapping.color.Equals(pixelColor))
                {
                    Vector2 position = new Vector2(x, y);

                // and instantiating prefab with left-top shift (hardcoded)
                Instantiate(colorMapping.prefab,
                                new Vector2(position.x - 3.5f,
                                            position.y + ScreenBound.bounds.y),
                                            Quaternion.identity, transform);

                }
            }   
        }



}





