using UnityEngine;

//The character as of now is 1.56m long in real world units or 0.78 in Unity capsule units. (normal height for 13 year old)
public class HumanPlayerController : MonoBehaviour
{
    [SerializeField] private Vector3 _velocityTest;
    [SerializeField] private bool _onGround;
    [SerializeField] private Camera _humanCamera;
    [SerializeField, Range(0f, 10f)] private float maxJumpHeight = 2f;
    [SerializeField, Range(0f, 50f)] private float _maxSpeed = 10f;
    [SerializeField, Range(0f, 25f)] private float pushPower = 3f;

    private CharacterController _controller;
    private Vector2 _playerInput;
    private float _ySpeed;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        getPlayerMovementInput();
        applyGravity();
        calculateVelocityAndMove();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        MovePushableObject(hit.collider.attachedRigidbody, hit);
    }

    //gets player movement input
    private void getPlayerMovementInput()
    {
        _playerInput.x = Input.GetAxis("HumanHorizontal");
        _playerInput.y = Input.GetAxis("HumanVertical");
    }

    //This function is reponsible for continuously applying gravity to our human.
    private void applyGravity()
    {
        _onGround = _controller.isGrounded;
        _ySpeed += Physics.gravity.y * Time.deltaTime;

        //if we set it to zero the _controller.isGrounded property does not work as intended so we must set it to a small negative value
        if (_onGround)
        {
            _ySpeed = -0.01f;
        }
    }

    //this function calculates the velocity magnitude and direction of the current movement and after makes the character move.
    //It is also responsible for listening to jumping input
    private void calculateVelocityAndMove()
    {
        if (_onGround && Input.GetButtonDown("HumanJump"))
        {
            Jump();
        }
        //get player input
        //get cameras forward and right vectors
        //multiply input X vector by camera right vector
        //multiply input Z vector by camera forward vector
        //add these two vectors

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

    //https://screenrec.com/share/62pUYiuKDW
    public void Jump()
    {
        _ySpeed += Mathf.Sqrt(maxJumpHeight * -2.0f * Physics.gravity.y);
    }

    //removes bounciness when moving down slopes, keeps the direction of the movement align with the slope angle
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

    //OnControllerColliderHit is called when the controller hits a collider while performing a Move.
    //We will use it to push any object with a rigidbody attached.
    //Later if we dont want to push all rigidbodies we can add a tag to the ones we want to move and check it before we apply the logic

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
