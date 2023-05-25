using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] protected string _character;
    protected CinemachineFreeLook _mainCamera;

    protected GameObject[] _TunnelCameraTriggers;

    protected const float DefaultTopRigRadius = 2.5f;
    protected const float DefaultMiddleRigRadius = 6f;
    protected const float DefaultBottomRigRadius = 2.5f;

    protected const float RoryDefaultTopRigHeight = 3.5f;
    protected const float RoryDefaultMiddleRigHeight = 2.5f;
    protected const float RoryDefaultBottomRigHeight = 0f;

    protected const float SpiritDefaultTopRigHeight = 3.5f;
    protected const float SpiritDefaultMiddleRigHeight = 1f;
    protected const float SpiritDefaultBottomRigHeight = -1f;

    protected const float TunnelTopRigRadius = 1;
    protected const float TunnelMiddleRigRadius = 2f;
    protected const float TunnelBottomRigRadius = 1f;

    void Awake()
    {
        _mainCamera = GetComponent<CinemachineFreeLook>();
        _TunnelCameraTriggers = GameObject.FindGameObjectsWithTag("ChangeCameraTunnelTrigger");
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
        _mainCamera.m_XAxis.m_InputAxisValue = playerCameraInput.x;
        _mainCamera.m_YAxis.m_InputAxisValue = playerCameraInput.y;
    }
}
