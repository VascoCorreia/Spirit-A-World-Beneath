using UnityEngine;

public class RoryMovement : MonoBehaviour
{
    [field: SerializeField] public Camera _camera { get; private set; }
    [field: SerializeField, Range(0f, 10f)] public float maxSpeed { get; set; } = 7f;
    [field: SerializeField] public float _ySpeed { get; private set; }
    [field: SerializeField] public static float _ySpeedInCurrentFrame { get; private set; }

    [SerializeField, Range(0f, 10f)] private float _maxJumpHeight = 2f;

    [SerializeField] private Vector3 _velocity;
    [SerializeField] private float FallingThreshold = -1f;

    private CharacterController _characterController;
    private float _lastPositionY;
    private Vector2 _playerInput;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _characterController.enabled = true;
    }

    private void Start()
    {
        _lastPositionY = transform.position.y;
    }

    private void Update()
    {
        getPlayerInput();
        IsFalling();
        applyGravity();
        calculateVelocityAndMove(_playerInput);
    }
    public void HandleMovement(Vector2 playerInput)
    {
        applyGravity();
        calculateVelocityAndMove(playerInput);
    }

    private void applyGravity()
    {
        _ySpeed += Physics.gravity.y * Time.deltaTime;

        //if we set it to zero the _controller.isGrounded property does not work as intended so we must set it to a small negative value
        if (_characterController.isGrounded)
        {
            _ySpeed = -0.01f;
        }
    }

    //this function calculates the velocity magnitude and direction of the current movement and after makes the character move.
    //It is also responsible for listening to jumping input
    private void calculateVelocityAndMove(Vector2 playerInput)
    {
        if (_characterController.isGrounded && Input.GetButtonDown("HumanJump") && !PushAndPullMechanic.isPulling)
        {
            Jump();
        }

        if (PushAndPullMechanic.isPulling)
        {
            MovementWhilePushingOrPulling(ref playerInput);
        }
        else
        {
            //get player input
            //get cameras forward and right vectors
            //multiply input X vector by camera right vector
            //multiply input Z vector by camera forward vector
            //add these two vectors

            Vector3 forward = _camera.transform.forward;
            Vector3 right = _camera.transform.right;
            forward.y = 0;
            right.y = 0;
            forward = forward.normalized;
            right = right.normalized;

            Vector3 forwardRelativeVerticalInput = forward * playerInput.y;
            Vector3 rightRelativeVerticalInput = right * playerInput.x;

            Vector3 cameraRelativeMovement = forwardRelativeVerticalInput + rightRelativeVerticalInput;

            float magnitudeTest = Mathf.Clamp01(cameraRelativeMovement.magnitude) * maxSpeed;

            cameraRelativeMovement.Normalize();
            _velocity = cameraRelativeMovement * magnitudeTest;

            _velocity.y = _ySpeed;
            _velocity = AdjustvelocityToSlope(_velocity);

            _characterController.Move(_velocity * Time.deltaTime);
        }
    }

    //Can only move backwards
    private void MovementWhilePushingOrPulling(ref Vector2 playerInput)
    {
        Vector3 forward = transform.forward;

        if(playerInput.y > 0)
        {
            playerInput.y = 0;
        }

        Vector3 forwardRelativeVerticalInput = forward * playerInput.y;

        Vector3 _velocity = forwardRelativeVerticalInput * maxSpeed;

        _velocity.y = _ySpeed;
        _velocity = AdjustvelocityToSlope(_velocity);

        _characterController.Move(_velocity * Time.deltaTime);
    }

    //https://screenrec.com/share/62pUYiuKDW
    private void Jump()
    {
        _ySpeed += Mathf.Sqrt(_maxJumpHeight * -2.0f * Physics.gravity.y);
    }

    //removes bounciness when moving down slopes, keeps the direction of the movement align with the slope angle
    private Vector3 AdjustvelocityToSlope(Vector3 velocity)
    {
        Debug.DrawRay(transform.position, Vector3.down);

        var ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 1.5f))
        {
            var rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            var adjustedvelocity = rotation * velocity;

            //if slope is over controller limit slide down
            if (Vector3.Angle(Vector3.up, hitInfo.normal) > _characterController.slopeLimit)
            {
                return rotation * Vector3.forward * Physics.gravity.y;
            }
            //only adjust the _velocity if were moving down a slope which means out Y component of velocity must be negative, otherwise dont change velocity
            if (adjustedvelocity.y < 0)
            {
                return adjustedvelocity;
            }
        }
        return velocity;
    }

    private bool IsFalling()
    {
        _ySpeedInCurrentFrame = (transform.position.y - _lastPositionY) / Time.deltaTime;

        _lastPositionY = transform.position.y;

        if (_ySpeedInCurrentFrame > FallingThreshold)
        {
            return false;
        }
        else
            return true;
    }

    private void getPlayerInput()
    {
        _playerInput.x = Input.GetAxis("HumanHorizontal");
        _playerInput.y = Input.GetAxis("HumanVertical");
    }
}
