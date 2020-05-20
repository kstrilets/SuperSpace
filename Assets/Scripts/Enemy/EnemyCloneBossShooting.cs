using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enemy Clone Boss shoots small clones
public class EnemyCloneBossShooting : MonoBehaviour
{
    // getting boss prefab, array of fire points and shooting delays
    [SerializeField] GameObject cloneFiredPrefab;
    [SerializeField] Transform[] firePoints;
    [SerializeField] float initDelay;
    [SerializeField] float repeatDelay;

    //Start shooting after initDelay, repeating after repeatDelay
    private void OnBecameVisible()
    {
        InvokeRepeating("ShootingAllField", initDelay, repeatDelay);
    }

    //Shooting from the whole array of fire points
    void ShootingAllField ()
    {
        foreach (Transform firePoint in firePoints)
        {
            Instantiate(cloneFiredPrefab, firePoint.position, firePoint.rotation);
        }
    }
}
