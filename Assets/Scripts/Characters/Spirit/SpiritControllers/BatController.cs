using UnityEngine;

public class BatController : MovableController
{
    [SerializeField] private float _rotationSpeed;

    void Start()
    {
        _maxSpeed = 12f;
        _rotationSpeed = 5f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Actions();
    }

    protected override void Actions()
    {
        Rotate();

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

    private void Rotate()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = _camera.transform.forward * playerInput.y;
        targetDirection = targetDirection + _camera.transform.right * playerInput.x;
        targetDirection.Normalize();

        if (targetDirection == Vector3.zero)
            targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bat" && other.gameObject != gameObject)
        {
            _spiritPossession.ExitPossession();
        }
    }
}

