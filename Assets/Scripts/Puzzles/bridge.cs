using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bridge : MonoBehaviour
{
    public void BridgeActive()
    {
        //PlaySound()
        //PlayAnimation()
        gameObject.SetActive(false);
    }

    public void BridgeClosed()
    {
        gameObject.SetActive(true);
    }
}