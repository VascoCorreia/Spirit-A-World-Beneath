using UnityEngine;

public class Bat : MonoBehaviour, IPossessable
{
    public string TypeInPossession { get; set; } = "Bat";
    public void Possess(possessionEventArgs raycastedBat)
    {
        //Only possess the correct bat
        if (raycastedBat.getPossessedEnemy() == gameObject)
        {
            gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            gameObject.AddComponent<CharacterController>();
            gameObject.AddComponent<BatController>();
            gameObject.GetComponent<BatAi>().enabled = false;
            gameObject.GetComponent<PlayerInteract>().enabled = true;
        }
    }

    public void ExitPossess()
    {
        Destroy(GetComponent<CharacterController>());
        Destroy(GetComponent<BatController>());
        gameObject.GetComponent<BatAi>().enabled = true;
        gameObject.GetComponent<PlayerInteract>().enabled = false;

        gameObject.tag = "Bat";
        gameObject.layer = LayerMask.NameToLayer("PossessableDynamic");
    }
}
