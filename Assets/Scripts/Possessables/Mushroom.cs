using UnityEngine;

public class Mushroom : MonoBehaviour, IPossessable
{
    public string TypeInPossession { get; set; } = "Mushroom";
    public void Possess(possessionEventArgs raycastedMushroom)
    {
        if (raycastedMushroom.getPossessedEnemy() == gameObject)
        {
            gameObject.GetComponent<MushroomController>().enabled = true;
        }
    }

    public void ExitPossess()
    {
        gameObject.GetComponent<MushroomController>().enabled = false;
    }

}
