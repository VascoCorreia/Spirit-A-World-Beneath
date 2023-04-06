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
            gameObject.AddComponent<BatController>();

            gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;

        }
    }

    public void ExitPossess()
    {
        Destroy(GetComponent<CharacterController>());
        Destroy(GetComponent<PlayerInteract>());
        Destroy(GetComponent<SpiritPlayerController>());

        gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
    }
}
