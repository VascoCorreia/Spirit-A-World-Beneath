using System;
using UnityEngine;

public class SpiritCameraController : CameraController
{
    [SerializeField] private SpiritPossession _spiritPossession;
    //[SerializeField] private CinemachineFreeLook _possessionCamera;
    private GameObject _spiritCached;

    private void OnEnable()
    {
        _spiritPossession.possessionSucessfull += ChangeCameraOnPossession;
        _spiritPossession.exitPossession += ChangeCameraOnExitPossession;

        foreach (GameObject trigger in _TunnelCameraTriggers)
        {
            trigger.GetComponent<ChangeToTunnelCameraTrigger>().spiritEnterTunnelCameraEvent += spiritEnterTunnelCameraEventHandler;
            trigger.GetComponent<ChangeToTunnelCameraTrigger>().spiritExitTunnelCameraEvent += spiritExitTunnelCameraEventHandler;
        }
    }

    private void OnDisable()
    {
        _spiritPossession.possessionSucessfull -= ChangeCameraOnPossession;
        _spiritPossession.exitPossession -= ChangeCameraOnExitPossession;

        foreach (GameObject trigger in _TunnelCameraTriggers)
        {
            trigger.GetComponent<ChangeToTunnelCameraTrigger>().spiritEnterTunnelCameraEvent -= spiritEnterTunnelCameraEventHandler;
            trigger.GetComponent<ChangeToTunnelCameraTrigger>().spiritExitTunnelCameraEvent -= spiritExitTunnelCameraEventHandler;
        }

    }

    void Start()
    {
        _spiritCached = GameObject.Find("Spirit");
    }

    void ChangeCameraOnPossession(possessionEventArgs possessedGameobject)
    {
        _mainCamera.Follow = possessedGameobject.getPossessedEnemy().transform;
        _mainCamera.LookAt = possessedGameobject.getPossessedEnemy().transform;

        //parent the possessed camera to the possessed gameobject
        transform.SetParent(possessedGameobject.getPossessedEnemy().transform);

        //Mushroom possession requires the camera to be further away from the character, since he can grow
        if (_spiritPossession.typeInPossession == "Mushroom")
        {
            _mainCamera.m_Orbits[0].m_Radius *= 1.5f;
            _mainCamera.m_Orbits[1].m_Radius *= 1.5f;
            _mainCamera.m_Orbits[2].m_Radius *= 1.5f;

            _mainCamera.m_Orbits[0].m_Height *= 2.5f;
            _mainCamera.m_Orbits[2].m_Height *= 0f;

        }

        if (_spiritPossession.typeInPossession == "Crystal")
        {
            _mainCamera.m_Orbits[2].m_Height *= 0f;
        }

        _mainCamera.Priority = 99;
    }

    private void ChangeCameraOnExitPossession()
    {
        //Return camera to spirit
        if (_character == "Spirit")
        {
            _mainCamera.Follow = _spiritCached.transform;
            _mainCamera.LookAt = _spiritCached.transform;
        }

        //Reset camera to regular values
        _mainCamera.m_Orbits[0].m_Radius = DefaultTopRigRadius;
        _mainCamera.m_Orbits[1].m_Radius = DefaultMiddleRigRadius;
        _mainCamera.m_Orbits[2].m_Radius = DefaultBottomRigRadius;

        _mainCamera.m_Orbits[0].m_Height = SpiritDefaultTopRigHeight;

        _mainCamera.Priority = 8;
    }
    private void spiritEnterTunnelCameraEventHandler(GameObject obj)
    {
        _mainCamera.m_Orbits[0].m_Radius = TunnelTopRigRadius;
        _mainCamera.m_Orbits[1].m_Radius = TunnelMiddleRigRadius;
        _mainCamera.m_Orbits[2].m_Radius = TunnelBottomRigRadius;

        _mainCamera.m_Orbits[0].m_Height = SpiritDefaultTopRigHeight;
        _mainCamera.m_Orbits[1].m_Height = SpiritDefaultMiddleRigHeight;
        _mainCamera.m_Orbits[2].m_Height = SpiritDefaultBottomRigHeight;
    }
    private void spiritExitTunnelCameraEventHandler(GameObject obj)
    {
        _mainCamera.m_Orbits[0].m_Radius = DefaultTopRigRadius;
        _mainCamera.m_Orbits[1].m_Radius = DefaultMiddleRigRadius;
        _mainCamera.m_Orbits[2].m_Radius = DefaultBottomRigRadius;

        _mainCamera.m_Orbits[0].m_Height = SpiritDefaultTopRigHeight;
        _mainCamera.m_Orbits[1].m_Height = SpiritDefaultMiddleRigHeight;
        _mainCamera.m_Orbits[2].m_Height = SpiritDefaultBottomRigHeight;
    }
}
