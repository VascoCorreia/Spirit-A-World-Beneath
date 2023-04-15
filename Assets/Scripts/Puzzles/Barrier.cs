using UnityEngine;

public class Barrier : MonoBehaviour
{
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
        gameObject.SetActive(true);
    }
}
