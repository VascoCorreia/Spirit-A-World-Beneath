using UnityEngine;

public class RoryMovement : MonoBehaviour
{
    [field: SerializeField] public Camera _camera { get; private set; }
    [field: SerializeField, Range(0f, 10f)] public float maxSpeed { get; set; } = 7f;
    [field: SerializeField] public float _ySpeed { get; private set; }

    public static float _ySpeedTest;
    [field: SerializeField] public static float _ySpeedInCurrentFrame { get; private set; }

    [SerializeField, Range(0f, 10f)] private float _maxJumpHeight = 2f;

    [SerializeField] private Vector3 _velocity;
    [SerializeField] private float FallingThreshold = 0.2f;

    private CharacterController _characterController;
    private float _lastPositionY;
    private Vector2 _playerInput;
    public bool isGrounded;
    public bool isFalling;
    public bool isSliding;
    public Transform testOrigin;
    public float maxDistance;

    //test for slope sliding
    public Vector3 hitNormal;

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
        //test sphere cast for grounded
        isGrounded = GroundSphereCast();

        _ySpeedTest = _ySpeed;
        isFalling = IsFalling();

        getPlayerInput();
        applyGravity();
        calculateVelocityAndMove(_playerInput);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;

        CheckIfPlayerIsSliding();
    }

    private void applyGravity()
    {
        _ySpeed += Physics.gravity.y * Time.deltaTime;

        //if we set it to zero the _controller.isGrounded property does not work as intended so we must set it to a small negative value
        if (isGrounded)
        {
            _ySpeed = -0.1f;
        }
    }

    //this function calculates the velocity magnitude and direction of the current movement and after makes the character move.
    //It is also responsible for listening to jumping input
    private void calculateVelocityAndMove(Vector2 playerInput)
    {
        if (isGrounded && Input.GetButtonDown("HumanJump") && !PushAndPullMechanic.isPulling)
        {
            Jump();
        }

        if (PushAndPullMechanic.isPulling)
        {
            MovementWhilePushingOrPulling(ref playerInput);
            //FMODUnity.RuntimeManager.PlayOneShot("event:/", GetComponent<Transform>().position);

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

            _velocity = AdjustvelocityToSlope(_velocity);
            _velocity.y += _ySpeed;

            _characterController.Move(_velocity * Time.deltaTime);
        }
    }

    //Can only move backwards
    private void MovementWhilePushingOrPulling(ref Vector2 playerInput)
    {
        Vector3 forward = transform.forward;

        if (playerInput.y > 0)
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
        //FMODUnity.RuntimeManager.PlayOneShot("event:/", GetComponent<Transform>().position);

    }

    //removes bounciness when moving down slopes, keeps the direction of the movement align with the slope angle
    private Vector3 AdjustvelocityToSlope(Vector3 velocity)
    {
        Debug.DrawRay(transform.position, Vector3.down * 0.1f);
        Quaternion rotation;
        Vector3 adjustedvelocity;
        var ray = new Ray(transform.position, Vector3.down);

        //Raycast used to check if a player is on a slope that he can walk on or not
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 0.1f))
        {
            /*Quaternion*/ rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            /*Vector3*/ adjustedvelocity = rotation * velocity;

            ////if slope is over controller limit slide down
            //if (Vector3.Angle(Vector3.up, hitNormal) > _characterController.slopeLimit)
            //{
            //    Debug.Log("Slide");
            //    adjustedvelocity = Vector3.ProjectOnPlane(new Vector3(0, _ySpeed, 0), hitNormal);

            //    adjustedvelocity = adjustedvelocity + velocity;                

            //    return adjustedvelocity;
            //}
            //else
            //{
            //    //only adjust the _velocity if were moving down a slope which means out Y component of velocity must be negative, otherwise dont change velocity
            //    if (adjustedvelocity.y < 0)

            //        return adjustedvelocity;
            //}

            return adjustedvelocity;
        }

        //if the character is sliding the movement must be different
        if(isSliding)
        {
            adjustedvelocity = Vector3.ProjectOnPlane(new Vector3(0, _ySpeed, 0), hitNormal);

            velocity = Vector3.zero;

            adjustedvelocity = adjustedvelocity + velocity;

            return adjustedvelocity;
        }

        Debug.Log("dont slide");
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
        //FMODUnity.RuntimeManager.PlayOneShot("event:/", GetComponent<Transform>().position);

    }

    private bool GroundSphereCast()
    {
        if (Physics.SphereCast(testOrigin.position, 0.13f, -transform.up, out RaycastHit hit, maxDistance))
        {
            Debug.Log(hit.collider);
            return true;
        }
        else
            return false;
    }

    private void OnDrawGizmos()
    {
        //debug sphere cast for grounded
        Gizmos.DrawWireSphere(testOrigin.position - transform.up * maxDistance, 0.13f);
    }

    private void CheckIfPlayerIsSliding()
    {
        float angle = Vector3.Angle(Vector3.up, hitNormal);

        if (!GroundSphereCast() && angle > _characterController.slopeLimit && _ySpeed < 0)
        {
            isSliding = true;
        }
        else
            isSliding = false;
    }
}

//e semrpe o vector3 forward o player q temm de ser cancelado no lado positivo