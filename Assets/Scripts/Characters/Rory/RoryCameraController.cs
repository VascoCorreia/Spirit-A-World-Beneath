using UnityEngine;

public class RoryCameraController : CameraController
{
    private void OnEnable()
    {
        foreach (GameObject trigger in _TunnelCameraTriggers)
        {
            trigger.GetComponent<ChangeToTunnelCameraTrigger>().roryEnterTunnelCameraEvent += roryEnterTunnelCameraEventHandler;
            trigger.GetComponent<ChangeToTunnelCameraTrigger>().roryExitTunnelCameraEvent += roryExitTunnelCameraEventHandler;
        }
    }

    private void OnDisable()
    {
        foreach (GameObject trigger in _TunnelCameraTriggers)
        {
            trigger.GetComponent<ChangeToTunnelCameraTrigger>().roryEnterTunnelCameraEvent -= roryEnterTunnelCameraEventHandler;
            trigger.GetComponent<ChangeToTunnelCameraTrigger>().roryExitTunnelCameraEvent -= roryExitTunnelCameraEventHandler;
        }

    }
    private void roryExitTunnelCameraEventHandler(GameObject obj)
    {
        _mainCamera.m_Orbits[0].m_Radius = DefaultTopRigRadius;
        _mainCamera.m_Orbits[1].m_Radius = DefaultMiddleRigRadius;
        _mainCamera.m_Orbits[2].m_Radius = DefaultBottomRigRadius;

        _mainCamera.m_Orbits[0].m_Height = RoryDefaultTopRigHeight;
        _mainCamera.m_Orbits[1].m_Height = RoryDefaultMiddleRigHeight;
        _mainCamera.m_Orbits[2].m_Height = RoryDefaultBottomRigHeight;
    }

    private void roryEnterTunnelCameraEventHandler(GameObject obj)
    {
        _mainCamera.m_Orbits[0].m_Radius = TunnelTopRigRadius;
        _mainCamera.m_Orbits[1].m_Radius = TunnelMiddleRigRadius;
        _mainCamera.m_Orbits[2].m_Radius = TunnelBottomRigRadius;

        _mainCamera.m_Orbits[0].m_Height = RoryDefaultTopRigHeight;
        _mainCamera.m_Orbits[1].m_Height = RoryDefaultMiddleRigHeight;
        _mainCamera.m_Orbits[2].m_Height = RoryDefaultBottomRigHeight;
    }
}
