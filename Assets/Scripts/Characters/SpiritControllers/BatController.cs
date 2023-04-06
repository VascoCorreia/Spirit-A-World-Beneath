using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : SpiritPlayerController
{
    // Start is called before the first frame update
    void Start()
    {
        _maxSpeed = 15f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Vector3 forward = _spiritCamera.transform.forward;
        Vector3 right = _spiritCamera.transform.right;

        Vector3 forwardRelativeVerticalInput = forward * _playerInput.y;
        Vector3 rightRelativeVerticalInput = right * _playerInput.x;

        Vector3 PlayerCameraRelativeMovement = forwardRelativeVerticalInput + rightRelativeVerticalInput;

        float magnitudeTest = Mathf.Clamp01(PlayerCameraRelativeMovement.magnitude) * _maxSpeed;

        PlayerCameraRelativeMovement.Normalize();
        _velocity = PlayerCameraRelativeMovement * magnitudeTest;
        _velocity = AdjustVelocityToSlope(_velocity);

        _controller.Move(_velocity * Time.deltaTime);
    }
}
