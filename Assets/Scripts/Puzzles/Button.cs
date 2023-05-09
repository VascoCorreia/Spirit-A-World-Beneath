using UnityEngine;

public class Button : MonoBehaviour, IInteractable
{
    [field: SerializeField] public Barrier associatedBarrier { get; private set; }
    public bool isInteracted { get; set; } = false;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Interacted(GameObject player)
    {
        isInteracted = true;
        //PlaySound
        //PlayAnimation
        if(player.CompareTag("Rory"))
        {
            player.GetComponent<RoryAnimatorManager>().PushingButtonAnimation();
            player.GetComponent<RoryAnimatorManager>().PushingButtonStartHandler();

        }
        _animator.SetTrigger("in");
        //SPIRIT ANIMATION??
        associatedBarrier.OpenBarrier();
    }

    public void Released(GameObject player)
    {
        if(isInteracted)
        {
            //PlaySound
            //PlayAnimation
            player.GetComponent<RoryAnimatorManager>().StopPushingButtonAnimation();
            player.GetComponent<RoryAnimatorManager>().PushingButtonEndHandler();
            _animator.SetTrigger("out");
            associatedBarrier.CloseBarrier();
        }
    }
}
