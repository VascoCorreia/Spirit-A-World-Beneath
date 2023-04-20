using UnityEngine;

public class MovableBox : MonoBehaviour, IInteractable
{
    public void Interacted(GameObject player)
    {
        PushAndPullMechanic.isPulling = true;
        transform.SetParent(player.transform, true);
    }

    public void Released(GameObject player)
    {
        PushAndPullMechanic.isPulling = false;
        transform.SetParent(null, true);

    }
}
