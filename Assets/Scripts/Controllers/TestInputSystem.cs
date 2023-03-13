using UnityEngine;
using UnityEngine.InputSystem;

public class TestInputSystem : MonoBehaviour
{
    private float _ySpeed;
    private Vector2 _playerInput;
    private CharacterController _controller;
    private PlayerInputAction _action;

    [SerializeField] private Camera _humanCamera;
    [SerializeField] private Vector3 _velocityTest;
    [SerializeField] private bool _onGround;
    [SerializeField, Range(0f, 10f)] private float maxJumpHeight = 2f;
    [SerializeField, Range(0f, 50f)] private float _maxSpeed = 10f;
    [SerializeField, Range(0f, 25f)] private float pushPower = 3f;


    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _action = new PlayerInputAction();
        _action.Player.Enable();
        _action.Player.Jump.performed += Jump;
    }

    void Update()
    {
        //_onGround = _controller.isGrounded; // inspector debuggging      
        calculateVelocity();
        applyGravity();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (_controller.isGrounded && context.performed)
            _ySpeed += Mathf.Sqrt(maxJumpHeight * -2.0f * Physics.gravity.y);
    }

    private void applyGravity()
    {
        _ySpeed += Physics.gravity.y * Time.deltaTime;

        //if we set it to zero the _controller.isGrounded property does not work as intended so we must set it to a small negative value
        if (_controller.isGrounded)
        {
            _ySpeed = -0.01f;
        }
    }
    public void calculateVelocity()
    {
        //get player input
        //get cameras forward and right vectors
        //multiply input X vector by camera right vector
        //multiply input Z vector by camera forward vector
        //add these two vectors

        _playerInput = _action.Player.Movement.ReadValue<Vector2>();

        Vector3 forward = _humanCamera.transform.forward;
        Vector3 right = _humanCamera.transform.right;
        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;

        Vector3 forwardRelativeVerticalInput = forward * _playerInput.y;
        Vector3 rightRelativeVerticalInput = right * _playerInput.x;

        Vector3 cameraRelativeMovement = forwardRelativeVerticalInput + rightRelativeVerticalInput;

        float magnitudeTest = Mathf.Clamp01(cameraRelativeMovement.magnitude) * _maxSpeed;

        cameraRelativeMovement.Normalize();
        _velocityTest = cameraRelativeMovement * magnitudeTest;
        _velocityTest.y = _ySpeed;
        _velocityTest = AdjustvelocityToSlope(_velocityTest);

        _controller.Move(_velocityTest * Time.deltaTime);
    }

    private Vector3 AdjustvelocityToSlope(Vector3 velocity)
    {
        Debug.DrawRay(transform.position, Vector3.down);

        var ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 2f))
        {
            var rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            var adjustedvelocity = rotation * velocity;

            //only adjust the _velocity if were moving down a slope which means out Y component of velocity must be negative, otherwise dont change velocity
            if (adjustedvelocity.y < 0)
            {
                return adjustedvelocity;
            }
        }
        return velocity;
    }

    private void MovePushableObject(Rigidbody body, ControllerColliderHit hit)
    {
        // no rigidbody nothing happens
        if (body == null || body.isKinematic)
        {
            return;
        }

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        //apply the push

        body.AddForce(pushDir * pushPower, ForceMode.Force);
    }
}
