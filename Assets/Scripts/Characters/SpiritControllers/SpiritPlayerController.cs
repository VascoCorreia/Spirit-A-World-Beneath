using Cinemachine;
using UnityEngine;

public class SpiritPlayerController : MonoBehaviour
{
    [field: SerializeField] public Camera _camera { get; private set; }
    [field: SerializeField] public CinemachineFreeLook _FreeLookCamera { get; private set; }

    protected Vector2 playerInput;

    protected virtual void OnEnable()
    {
        _camera = GameObject.Find("SpiritCameraBrain").GetComponent<Camera>();
        _FreeLookCamera = GameObject.Find("SpiritCamera").GetComponent<CinemachineFreeLook>();
    }

    protected virtual void Update()
    {
        return;
    }

    //All child classes require this inputs
    protected virtual void getPlayerInput()
    {
        playerInput.x = Input.GetAxis("SpiritHorizontal");
        playerInput.y = Input.GetAxis("SpiritVertical");
    }

    protected virtual void Actions()
    {
        return;
    }
}
