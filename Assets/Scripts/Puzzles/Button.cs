using UnityEngine;

public class Button : MonoBehaviour, IInteractable
{
    [field: SerializeField] public Barrier associatedBarrier { get; private set; }
    public bool isInteracted { get; set; } = false;

    public void Interacted(GameObject player)
    {
        isInteracted = true;
        //PlaySound
        //PlayAnimation
        if(player.CompareTag("Rory"))
        {
            player.GetComponent<RoryAnimatorManager>().pushingButtonAnimation();

        }

        //SPIRIT ANIMATION??
        associatedBarrier.OpenBarrier();
    }

    public void Released(GameObject player)
    {
        if(isInteracted)
        {
            //PlaySound
            //PlayAnimation
            associatedBarrier.CloseBarrier();
        }
    }
}
