using System;
using UnityEngine;

public class Death : MonoBehaviour
{
    public static Action playerDied;
    public bool eventFiredAlready;

    private void Start()
    {
        eventFiredAlready = false;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Bat"))
        {  
            if (!eventFiredAlready)
            {
                //Fire Event
                playerDied?.Invoke();
                eventFiredAlready = true;
            }
        }
    }
}
