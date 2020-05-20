using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//enemies are shooting here
public class EnemyShooting : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;       //chosing ammo prefab
    [SerializeField] Transform firePoint;           //getting coord of fire point

    [SerializeField] bool bombShooting;             //if shooting with bombs
    [SerializeField] int bombNumber;                //could be diff number
    [SerializeField] float bombDelay;               //with dropping delay

    [SerializeField] bool row;                      //shooting in packs
    [SerializeField] int rowCount;                  //number of bullets in pack
    [SerializeField] float rowRate;                 //pack rate
    [SerializeField] float rowDelay;                //delay between packs

    [SerializeField] bool normal;                   //normal shooting with random rate
    [SerializeField] float fireRateMin;
    [SerializeField] float fireRateMax;

    [SerializeField] bool radiantShooting;          //radiant shooting
    [SerializeField] int _radiantBulletsNumber;
    [SerializeField] bool isRadiantBNRandom;
    [SerializeField] int minRadiantBulletsNumber;
    [SerializeField] int maxRadiantBulletNumber;
    [SerializeField] float _firstShotDelay;
    [SerializeField] float _radiantBulletSpeed;
    [SerializeField] float _shotRate;
    Vector3 startPoint;

    //if enemy is visible we are checking what kind of shooting they will do
    //and ivoking corresponding methods or starting coroutines
    void OnBecameVisible()
    {
        if (bombShooting)
        {
           for (int i = 0; i < bombNumber; i++)
            {
                Invoke("FireBombs", bombDelay);
            }
        }

        if (row)
        {
            StartCoroutine(PackShoot(rowCount, rowRate, rowDelay));
        }

        if (normal)
        {
            float fireRateEnemy = Random.Range(fireRateMin, fireRateMax);
            InvokeRepeating("FireBullet", 0f, fireRateEnemy);
        }

        if (radiantShooting)
        {
            startPoint = transform.position;
            StartCoroutine(RadiantShot(_firstShotDelay, _shotRate));
        }
    }

    //simple shooting 
    void FireBullet()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    //shooting with bombs
    void FireBombs()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    //radiant shooting coroutine
    IEnumerator RadiantShot (float firstShotDelay, float shotRate)
    {
        const float radius = 3f;
        int radiantBulletsNumber;
        float angle = 0f;

        yield return new WaitForSeconds(firstShotDelay);

        WaitForSeconds delayShotRate = new WaitForSeconds(shotRate);

        while (true)
        {
            if (isRadiantBNRandom)
            {
                radiantBulletsNumber = Random.Range(minRadiantBulletsNumber, maxRadiantBulletNumber);
            }
            else
            {
                radiantBulletsNumber = _radiantBulletsNumber;
            }

            float angleStep = 360f / radiantBulletsNumber;
            for (int i = 0; i < radiantBulletsNumber; i++)
            {
                float bulletXPos = startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
                float bulletYPos = startPoint.y + Mathf.Cos((angle * Mathf.PI) / 180) * radius;

                Vector3 bulletVector = new Vector3(bulletXPos, bulletYPos, 0);
                Vector3 bulletMoveDir = (bulletVector - startPoint).normalized * _radiantBulletSpeed;

                GameObject _bulletPrefab = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                Rigidbody2D rb = _bulletPrefab.GetComponent<Rigidbody2D>();
                EnemyBulletRadiantMovement move =  new EnemyBulletRadiantMovement();
                move.Move(rb, bulletMoveDir, _radiantBulletSpeed);

                angle += angleStep;
            }
            yield return delayShotRate;
        }
    }

    //shooting in rows 
    IEnumerator PackShoot (int bulletsNumber, float delayBtwBullets, float delay)
    {
        WaitForSeconds delayDelay = new WaitForSeconds(delay);

        while (true)
            {
            for (int i = 0; i < bulletsNumber; i++)
            {
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                yield return new WaitForSeconds(delayBtwBullets);
            }
            yield return delayDelay;
        }
    }
}
