using System;
using UnityEngine;

//This class handles the static death event.
public class Death : MonoBehaviour
{
    public static Action playerDied;
    public bool eventFiredAlready;


    private void Start()
    {
        eventFiredAlready = false;
    }
    private void Update()
    {
       
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Bat"))
        {  
            if (!eventFiredAlready && !Cheats._invicibility)
            {
                //Fire Event
                playerDied?.Invoke();
                eventFiredAlready = true;
            }
        }
    }
}
