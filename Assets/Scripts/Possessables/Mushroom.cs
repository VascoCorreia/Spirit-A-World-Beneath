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
        gameObject.tag = "Mushroom";
        gameObject.layer = LayerMask.NameToLayer("PossessableStatic");

        Transform[] _allMushroom = gameObject.GetComponentsInChildren<Transform>();

        foreach(Transform t in _allMushroom)
        {
            t.gameObject.layer = LayerMask.NameToLayer("PossessableStatic");
        }
    }
}
