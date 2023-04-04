using System;
using UnityEngine;

public class Bat : MonoBehaviour, IPossessable
{
    public void Possess(possessionEventArgs raycastedBat)
    {
        //Only possess the correct bat
        if (raycastedBat.getPossessedEnemy() == gameObject)
        {
            gameObject.AddComponent<CharacterController>();
            gameObject.AddComponent<PlayerInteract>();
            gameObject.AddComponent<SpiritPlayerController>();

            gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;

        }
    }

    //When exiting possession:
    // 1- exable spirit gameobject
    // 2- Change camera
    // 3- disable this script
    public void ExitPossess()
    {
        Destroy(GetComponent<CharacterController>());
        Destroy(GetComponent<PlayerInteract>());
        Destroy(GetComponent<SpiritPlayerController>());

        gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
    }
}
