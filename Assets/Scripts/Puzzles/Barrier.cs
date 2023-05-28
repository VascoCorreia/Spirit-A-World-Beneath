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
        //PlayAnimation()
        _animator.SetTrigger("Open");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Others/Barrier Open", GetComponent<Transform>().position);

        //gameObject.SetActive(false);
    }

    public void CloseBarrier()
    {
        //PlayAnimation()
        FMODUnity.RuntimeManager.PlayOneShot("event:/Others/Close Barrier", GetComponent<Transform>().position);
        if (!staysOpen)
        {
            _animator.SetTrigger("Close");
        }
    }
}
