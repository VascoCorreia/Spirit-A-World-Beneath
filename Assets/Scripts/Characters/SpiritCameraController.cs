using Cinemachine;
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
    }

    private void OnDisable()
    {
        _spiritPossession.possessionSucessfull -= ChangeCameraOnPossession;
        _spiritPossession.exitPossession -= ChangeCameraOnExitPossession;
    }

    void Start()
    {
        _spiritCached = GameObject.Find("Spirit");
    }

    void ChangeCameraOnPossession(possessionEventArgs possessedGameobject)
    {
        _camera.Follow = possessedGameobject.getPossessedEnemy().transform;
        _camera.LookAt = possessedGameobject.getPossessedEnemy().transform;

        //parent the possessed camera to the possessed gameobject
        transform.SetParent(possessedGameobject.getPossessedEnemy().transform);

        //Mushroom possession requires the camera to be further away from the character, since he can grow
        if (_spiritPossession.typeInPossession == "Mushroom")
        {
            _camera.m_Orbits[0].m_Radius *= 1.5f;
            _camera.m_Orbits[1].m_Radius *= 1.5f;
            _camera.m_Orbits[2].m_Radius *= 1.5f;

            _camera.m_Orbits[0].m_Height *= 2.5f;
            _camera.m_Orbits[2].m_Height *= 0f;

        }

        _camera.Priority = 99;
    }

    private void ChangeCameraOnExitPossession()
    {
        //Return camera to spirit
        if (_character == "Spirit")
        {
            _camera.Follow = _spiritCached.transform;
            _camera.LookAt = _spiritCached.transform;
        }

        //Reset camera to regular values
        _camera.m_Orbits[0].m_Radius = DefaultTopRigRadius;
        _camera.m_Orbits[1].m_Radius = DefaultMiddleRigRadius;
        _camera.m_Orbits[2].m_Radius = DefaultBottomRigRadius;

        _camera.m_Orbits[0].m_Height = DefaultTopRigHeight;

        _camera.Priority = 8;
    }
}
