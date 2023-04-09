using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour, IPossessable
{
    public string TypeInPossession { get; set; } = "Crystal";

    public void ExitPossess()
    {
        gameObject.GetComponent<CrystalController>().enabled = false;
    }

    public void Possess(possessionEventArgs raycastedCrystal)
    {
        if (raycastedCrystal.getPossessedEnemy() == gameObject)
        {
            gameObject.GetComponent<CrystalController>().enabled = true;

        }
    }
}
