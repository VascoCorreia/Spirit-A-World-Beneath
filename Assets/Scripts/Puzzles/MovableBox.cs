using UnityEngine;

public class MovableBox : MonoBehaviour, IInteractable
{
    public void Interacted(GameObject player)
    {
        //The player can only use mechanic when grounded
        if (player.GetComponent<CharacterController>().isGrounded && player.CompareTag("Rory"))
        {
            PushAndPullMechanic.isPulling = true;

            //parent object to player to simulate pushing and pulling
            transform.SetParent(player.transform, true);
        }
    }

    public void Released(GameObject player)
    {
        if (player.CompareTag("Rory"))
        {
            PushAndPullMechanic.isPulling = false;
            transform.SetParent(null, true);
        }
    }
}
