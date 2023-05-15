using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    protected CinemachineFreeLook _camera;
    [SerializeField] protected string _character;

    protected const float DefaultTopRigRadius = 4.5f;
    protected const float DefaultMiddleRigRadius = 9f;
    protected const float DefaultBottomRigRadius = 4.5f;
    protected const float DefaultTopRigHeight = 4.5f;

    void Awake()
    {
        _camera = GetComponent<CinemachineFreeLook>();
    }

    void Update()
    {
        MoveCamera(_character);
    }

    private void MoveCamera(string character)
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
}
