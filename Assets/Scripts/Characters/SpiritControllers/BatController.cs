using UnityEngine;

public class BatController : MovableController
{
    void Start()
    {
        _maxSpeed = 15f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Actions();
    }

    protected override void Actions()
    {
        Vector3 forward = _camera.transform.forward;
        Vector3 right = _camera.transform.right;

        Vector3 forwardRelativeVerticalInput = forward * playerInput.y;
        Vector3 rightRelativeVerticalInput = right * playerInput.x;

        Vector3 PlayerCameraRelativeMovement = forwardRelativeVerticalInput + rightRelativeVerticalInput;

        float magnitudeTest = Mathf.Clamp01(PlayerCameraRelativeMovement.magnitude) * _maxSpeed;

        PlayerCameraRelativeMovement.Normalize();
        _velocity = PlayerCameraRelativeMovement * magnitudeTest;
        _velocity = AdjustVelocityToSlope(_velocity);

        _controller.Move(_velocity * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.collider.tag == "Bat")
        {
            _spiritPossession.ExitPossession();
        }
    }
}
