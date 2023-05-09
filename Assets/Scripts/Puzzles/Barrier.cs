using UnityEngine;

public class Barrier : MonoBehaviour
{
    [SerializeField] private bool staysOpen;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public void OpenBarrier()
    {
        //PlaySound()
        //PlayAnimation()
        _animator.SetTrigger("Open");
        //gameObject.SetActive(false);
    }

    public void CloseBarrier()
    {
        //PlaySound()
        //PlayAnimation()
        if (!staysOpen)
        {
            _animator.SetTrigger("Close");
        }
    }
}
