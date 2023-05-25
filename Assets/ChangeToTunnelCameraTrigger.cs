using System;
using UnityEngine;

public class ChangeToTunnelCameraTrigger : MonoBehaviour
{
    public Action<GameObject> roryEnterTunnelCameraEvent, roryExitTunnelCameraEvent, spiritEnterTunnelCameraEvent, spiritExitTunnelCameraEvent;

    private void OnTriggerEnter(Collider player)
    {
        if(player.transform.parent.CompareTag("Rory"))
        {
            roryEnterTunnelCameraEvent?.Invoke(player.gameObject);
        }

        if(player.transform.parent.CompareTag("Spirit"))
        {
            spiritEnterTunnelCameraEvent?.Invoke(player.gameObject);
        }
    }

    private void OnTriggerExit(Collider player)
    {
        if (player.transform.parent.CompareTag("Rory"))
        {
            roryExitTunnelCameraEvent?.Invoke(player.gameObject);
        }

        if(player.transform.parent.CompareTag("Spirit"))
        {
            spiritExitTunnelCameraEvent?.Invoke(player.gameObject);
        }
    }
}