using UnityEngine;

public class MovableBox : MonoBehaviour, IInteractable
{
    public void Interacted(GameObject player)
    {
        PushAndPullMechanic.isPulling = true;

        //make player look at object
        player.transform.LookAt(transform.position);

        //parent object to player to simulate pushing and pulling
        transform.SetParent(player.transform, true);
    }

    public void Released(GameObject player)
    {
        PushAndPullMechanic.isPulling = false;
        transform.SetParent(null, true);
    }
}
