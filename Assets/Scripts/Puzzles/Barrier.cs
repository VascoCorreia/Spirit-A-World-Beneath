using UnityEngine;

public class Barrier : MonoBehaviour
{
    public void OpenBarrier()
    {
        //PlaySound()
        //PlayAnimation()
        Debug.Log("im opening!");
        gameObject.SetActive(false);
    }

    public void CloseBarrier()
    {
        //PlaySound()
        //PlayAnimation()
        Debug.Log("closing!");
        gameObject.SetActive(true);
    }
}
