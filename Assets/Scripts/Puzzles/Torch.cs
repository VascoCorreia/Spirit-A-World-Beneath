using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour, IInteractable
{

    //TOCHA MUST BE IN LAYER INTERACTABLE
    public void Interacted(GameObject player)
    {
        if(player.tag == "Rory")
        {
           //tocha vai para rory
           //
        }
    }

    public void Released(GameObject player)
    {
        if (player.tag == "Rory")
        {
            // IF RORY NOT LOOKING AT PLACE TO PUT TOCHA OU ESTIVER LONGE DESSA MERDA -> TOCHA CAI
           
            //IF RORY IS LOOKING AT PLACE TO PUT TOCHA AND IS CLOSE ENOUGH -> DEBUG.LOG("I AM WORKING")

            //IF RORY IS TOO FAR AWAY -> TOCHA CAI

            //IF RORY NOT LOOKING -> TOCHA CAI


        }
    }
}
