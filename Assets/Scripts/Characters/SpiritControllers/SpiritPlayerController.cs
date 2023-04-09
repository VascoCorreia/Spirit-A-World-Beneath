using Cinemachine;
using UnityEngine;

public class SpiritPlayerController : MonoBehaviour
{
    [field: SerializeField] public Camera _camera { get; private set; }
    [field: SerializeField] public CinemachineFreeLook _FreeLookCamera { get; private set; }

    protected Vector2 _playerInput;

    protected virtual void OnEnable()
    {
        _camera = GameObject.Find("SpiritCameraBrain").GetComponent<Camera>();
        _FreeLookCamera = GameObject.Find("SpiritCamera").GetComponent<CinemachineFreeLook>();
    }

    protected virtual void Update()
    {
        return;
    }

    protected virtual void getPlayerInput()
    {
        _playerInput.x = Input.GetAxis("SpiritHorizontal");
        _playerInput.y = Input.GetAxis("SpiritVertical");
    }

    protected virtual void Actions()
    {
        return;
    }
}
