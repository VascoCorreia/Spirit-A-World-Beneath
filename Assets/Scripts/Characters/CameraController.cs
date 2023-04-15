using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CinemachineFreeLook _camera;
    [SerializeField] private string _character;
    [SerializeField] private SpiritPossession _spiritPossession;
    [SerializeField] private GameObject _spiritCached;

    private const float DefaultTopRigRadius = 4.5f;
    private const float DefaultMiddleRigRadius = 9f;
    private const float DefaultBottomRigRadius = 4.5f;
    private const float DefaultTopRigHeight = 4.5f;

    void Awake()
    {
        _camera = GetComponent<CinemachineFreeLook>();
        _spiritCached = GameObject.Find("Spirit");
    }

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
    void Update()
    {
        MoveCamera(_character);
    }

    public void MoveCamera(string character)
    {
        Vector2 playerCameraInput = Vector2.zero;

        if (character == "Rory")
        {
            playerCameraInput = new Vector2(Input.GetAxis("HumanCameraHorizontal"), Input.GetAxis("HumanCameraVertical"));
        }

        if (character == "Spirit")
        {
            playerCameraInput = new Vector2(Input.GetAxis("SpiritCameraHorizontal"), Input.GetAxis("SpiritCameraVertical"));
        }

        //change manually the fields of the frelookcamera
        _camera.m_XAxis.m_InputAxisValue = playerCameraInput.x;
        _camera.m_YAxis.m_InputAxisValue = playerCameraInput.y;
    }

    void ChangeCameraOnPossession(possessionEventArgs possessedGameobject)
    {
        if (_character == "Spirit")
        {
            _camera.Follow = possessedGameobject.getPossessedEnemy().transform;
            _camera.LookAt = possessedGameobject.getPossessedEnemy().transform;


            //Mushroom possession requires the camera to be further away from the character, since he can grow
            if (_spiritPossession.typeInPossession == "Mushroom")
            {
                _camera.m_Orbits[0].m_Radius *= 1.5f;
                _camera.m_Orbits[1].m_Radius *= 1.5f;
                _camera.m_Orbits[2].m_Radius *= 1.5f;

                _camera.m_Orbits[0].m_Height *= 2.5f;

            }
        }
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

    }
}
