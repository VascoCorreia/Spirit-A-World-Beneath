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

    //When Rory hits a bat fire an event once that triggers death
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Bat"))
        {
            if(!eventFiredAlready)
            {
                //Fire Event
                playerDied?.Invoke();
                eventFiredAlready = true;
            }
        }
    }
}
