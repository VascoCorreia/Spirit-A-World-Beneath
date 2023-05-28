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

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Bat"))
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Players/Rory Death", GetComponent<Transform>().position);

            if (!eventFiredAlready && !Cheats._invicibility)
            {
                //Fire Event
                playerDied?.Invoke();
                eventFiredAlready = true;
            }
        }
    }
}
