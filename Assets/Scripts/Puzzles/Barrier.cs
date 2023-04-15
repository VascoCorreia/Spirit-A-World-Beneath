using UnityEngine;

public class Barrier : MonoBehaviour
{
    [SerializeField] private bool staysOpen;
    public void OpenBarrier()
    {
        //PlaySound()
        //PlayAnimation()
        gameObject.SetActive(false);
    }

    public void CloseBarrier()
    {
        //PlaySound()
        //PlayAnimation()
        if(!staysOpen)
        {
            gameObject.SetActive(true);
        }
    }
}
