using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour, IPossessable
{
    public string TypeInPossession { get; set; } = "Crystal";

    public void ExitPossess()
    {
        gameObject.GetComponent<CrystalController>().enabled = false;
        gameObject.GetComponent<CrystalController>().DisableLights();

    }

    public void Possess(possessionEventArgs raycastedCrystal)
    {
        if (raycastedCrystal.getPossessedEnemy() == gameObject)
        {
            gameObject.GetComponent<CrystalController>().enabled = true;
            gameObject.GetComponent<CrystalController>().EnableLights();

        }
    }
}
