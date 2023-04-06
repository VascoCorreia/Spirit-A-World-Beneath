using UnityEngine;

public class Mushroom : MonoBehaviour, IPossessable
{
    public void ExitPossess()
    {
        gameObject.GetComponent<CharacterController>().enabled = false;
        gameObject.GetComponent<MushroomController>().enabled = false;
    }

    public void Possess(possessionEventArgs raycastedMushroom)
    {
        if (raycastedMushroom.getPossessedEnemy() == gameObject)
        {
            gameObject.GetComponent<CharacterController>().enabled = true;
            gameObject.GetComponent<MushroomController>().enabled = true;
        }
    }
}
