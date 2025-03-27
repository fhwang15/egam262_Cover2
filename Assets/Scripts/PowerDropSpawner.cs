using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class PowerDropSpawner : MonoBehaviour
{

    public GameObject powerDrop;


    //Once called, it will drop the power up Prefab.
    //I feel like it will be attached to the enemy that is... red? special? idk
    //

    public void dropPowerUp()
    {
        Transform theDropped; //<This is not doing it's job

        if (powerDrop != null)
        {
            Instantiate(powerDrop);

        }
    }
}
