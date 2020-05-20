using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{

    //player bullets for different fire points and different fire bonuses
    [SerializeField] GameObject sideBulletPrefab;
    [SerializeField] GameObject farSideBulletPrefab;
    [SerializeField] GameObject centerBulletPrefab;

    //player's fire point
    [SerializeField] Transform firePointLeft;
    [SerializeField] Transform firePointRight;
    [SerializeField] Transform firePointCenter;
    [SerializeField] Transform firePointLeftLeft;
    [SerializeField] Transform firePointRightRight;

    //Base player bullet Rate
    [SerializeField] float baseBulletRate;

    [SerializeField] float centerBaseBulletRate;
    [SerializeField] float farBaseBulletRate;


    //Fire bonuses bools
    public static bool fireBonus0 = true;
    public static bool fireBonus1;
    public static bool fireBonus2;
    public static bool fireBonus3;

    //is player has full fire bonus
    public static bool fullChargedBonus;

    public static bool fireRateBonus;

    //aux bullets variable
    float nextBullet;
    float nextCenterBullet;
    float nextFarBullet;
    float bulletRate;
    float centerBulletRate;
    float farBulletRate;

    //aux bullets bools
    bool centerBullet = true;
    bool bullet = true;
    bool farBullet = true;

    //is coroutine is running bool
    bool _crBonusRunning;
    bool _crTimerRunning;

    IEnumerator cRT;

    private void Start()
    {
        //setting bullet rate to base rate
        bulletRate = baseBulletRate;
        centerBulletRate = centerBaseBulletRate;
        farBulletRate = farBaseBulletRate;
        fireRateBonus = false;
        if (_crBonusRunning)
            StopCoroutine(BulletRateBonus());

        //Starting coroutine to decrese Fire Bonus in time
        //StartCoroutine(BulletRateTimer());
    }

    private void Update()
    {

        if (PlayerState.isFireBonusChanged)
        {
            if (cRT != null)
            {
                StopCoroutine(cRT);
            }
            cRT = BulletRateTimer();
            StartCoroutine(cRT);
            PlayerState.isFireBonusChanged = false;
        }
        // cheking which fire bonus to apply
        if (fireBonus0)
        {
            Fire_1();            
        }

        if (fireBonus1)
        {
            Fire_2();
        }

        if (fireBonus2)
        {
            Fire_3();
        }

        if (fireBonus3)
        {
            Fire_4();
        }
    }


    // coroutine for charge decr 1 point after delay  
    IEnumerator BulletRateTimer()
    {
        WaitForSecondsRealtime delay = new WaitForSecondsRealtime(14);

        while (PlayerState.playerHealth > 0)
        {
            yield return delay;

            EventManager.TriggerEvent("FIREBONUSDECREMENT");

            yield return null;
        }
    }


    //coroutine for applying 3 sec of fire rate bonus with check to run it only ones
    IEnumerator BulletRateBonus()
    {
        WaitForSeconds delay = new WaitForSeconds(3f);
        _crBonusRunning = true;

        while (fireRateBonus)
        {
            bulletRate = 0.03f;
            centerBulletRate = bulletRate * 2f;
            farBulletRate = bulletRate;
            yield return delay;
            fireRateBonus = false;
            bulletRate = baseBulletRate;
            centerBulletRate = centerBaseBulletRate;
            farBulletRate = farBaseBulletRate;
        }

        yield return new WaitForEndOfFrame();
        _crBonusRunning = false;
    }


    void Fire_1()
    {

        if (fireRateBonus && _crBonusRunning == false)  //cheking is rate bonus was applyed
            StartCoroutine(BulletRateBonus());

        // instantiating bullets from one-by-one left and right sides of the ship (closer fire points)
        if (Time.time > nextBullet && bullet == true)
        {
            nextBullet = Time.time + bulletRate;
            Instantiate(sideBulletPrefab, firePointRight.position, firePointRight.rotation);
            bullet = false;
        }
        if (Time.time > nextBullet + bulletRate && bullet == false)
        {
            nextBullet = Time.time + bulletRate;
            Instantiate(sideBulletPrefab, firePointLeft.position, firePointLeft.rotation);
            bullet = true;
        }
    }

  
    void Fire_2()
    {

        if (fireRateBonus && _crBonusRunning == false)  //cheking is rate bonus was applyed
            StartCoroutine(BulletRateBonus());

        // instantiating bullets from the center of the ship
        if (Time.time > nextCenterBullet && centerBullet == true)
        {
            nextCenterBullet = Time.time + centerBulletRate;
            Instantiate(centerBulletPrefab, firePointCenter.position, firePointCenter.rotation);
            centerBullet = false;
        }
        if (Time.time > nextCenterBullet + centerBulletRate && centerBullet == false)
        {
            nextCenterBullet = Time.time + centerBulletRate;
            Instantiate(centerBulletPrefab, firePointCenter.position, firePointCenter.rotation);
            centerBullet = true;
        }
    }

   
    void Fire_3()
    {
        if (fireRateBonus && _crBonusRunning == false)    //cheking is rate bonus was applyed
            StartCoroutine(BulletRateBonus());


        // instantiating bullets from one-by-one left and right sides of the ship (far fire points)
        if (Time.time > nextFarBullet && farBullet == true)
        {
            nextFarBullet = Time.time + farBulletRate;
            Instantiate(farSideBulletPrefab, firePointRightRight.position, firePointRightRight.rotation);
            farBullet = false;
        }

        if (Time.time > nextFarBullet + farBulletRate && farBullet == false)
        {
            nextFarBullet = Time.time + farBulletRate;
            Instantiate(farSideBulletPrefab, firePointLeftLeft.position, firePointLeftLeft.rotation);
            farBullet = true;
        }
    }


    //coroutine for full player charge bonus so it is lasts only 2 sec
    void Fire_4()
    {
        StartCoroutine(FullChargeBonusCoroutine());
    }

    IEnumerator FullChargeBonusCoroutine()
    {
        WaitForSeconds delay = new WaitForSeconds(2f);
        fullChargedBonus = true;

        yield return delay;

        fullChargedBonus = false;
    }
}
