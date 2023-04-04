using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CinemachineFreeLook _camera;
    [SerializeField] private string _character;
    [SerializeField] private SpiritPossession _spiritPossession;
    [SerializeField] private GameObject _spiritCached;

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
        //get input from unity input system

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
        }
    }
    private void ChangeCameraOnExitPossession()
    {
        if (_character == "Spirit")
        {
            _camera.Follow = _spiritCached.transform;
            _camera.LookAt = _spiritCached.transform;
        }

    }
}
