using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Purple Boss is shooting!

public class EnemyBossPurpleShooting : MonoBehaviour
{
    // Getting Purple Boss bullet to shoot
    [SerializeField] GameObject bullet;

    // Setting an array of Purple Boss shooting points
    [SerializeField] Transform[] firePosition;

    int count;
  
    void Start()
    {
        count = 0;
        // Firing

        InvokeRepeating("Fire", 1f, 0.7f);
    }

    // Instantianting a bullet at a random fire point
    void Fire()
    {
        count++;

        if (count == 10)
        {
            count = Random.Range(0, 3);
            foreach (var fireP in firePosition)
            {
                Instantiate(bullet, fireP.position, Quaternion.identity);

            }
        } else
        {
            int i = Random.Range(0, firePosition.Length - 1);
            Instantiate(bullet, firePosition[i].position, Quaternion.identity);
        }
        
    }
}
