using System;
using UnityEngine;


//This class is used for Spirit controllers that require translation mainly
public class MovableController : SpiritPlayerController
{
    [field: SerializeField] public PlayerInteract _playerInteract { get; private set; }
    //[field: SerializeField] public SpiritPossession _spiritPossession { get; private set; }
    //[field: SerializeField] public CharacterController _controller { get; private set; }
    [field: SerializeField, Range(0f, 50f)] public float _maxSpeed { get; set; }

    [SerializeField, Range(0f, 10f)] private float maxJumpHeight;
    [SerializeField] protected Vector3 _velocity;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private bool _isSliding;
    [SerializeField] private Transform _originForGroundSphereCast;
    [SerializeField] private float _maxDistanceForSphereGroundCheck;
    [SerializeField] private float _radiusSphereGroundCheck;

    private float _ySpeed;
    private Vector3 hitNormal;

    protected override void OnEnable()
    {
        base.OnEnable();
        //_controller = GetComponent<CharacterController>();
        _playerInteract = GetComponent<PlayerInteract>();
        //_spiritPossession = GameObject.Find("Possession").GetComponent<SpiritPossession>();
    }

    protected override void Update()
    {
        base.Update();
        GetPlayerInput();

        if (_spiritPossession.typeInPossession == null)
        {
            _isGrounded = GroundSphereCastGroundCheck();
            ApplyGravity();
            Actions();
        }
    }

    //gets player movement input
    protected override void GetPlayerInput()
    {
        base.GetPlayerInput();
        ////R1
        //if (Input.GetButtonDown("SpiritPossession"))
        //{
        //    _spiritPossession.tryPossession();
        //    //FMODUnity.RuntimeManager.PlayOneShot("event:/", GetComponent<Transform>().position);
        //}
        ////R2
        //if (Input.GetButtonDown("SpiritExitPossession"))
        //{
        //    _spiritPossession.ExitPossession();
        //    //FMODUnity.RuntimeManager.PlayOneShot("event:/", GetComponent<Transform>().position);

        //}
        //Square
        if (Input.GetButtonDown("SpiritInteract"))
        {
            _playerInteract.Interact(_camera);
        }
        if (Input.GetButtonUp("SpiritInteract"))
        {
            _playerInteract.StopInteract();
        }
    }
    //This function is reponsible for continuously applying gravity to our character.
    private void ApplyGravity()
    {
        _ySpeed += Physics.gravity.y * Time.deltaTime;

        //if we set it to zero the _controller.isGrounded property does not work as intended so we must set it to a small negative value
        if (_isGrounded)
        {
            _ySpeed = -0.01f;
        }
    }

    //https://screenrec.com/share/62pUYiuKDW
    private void Jump()
    {
        _ySpeed += Mathf.Sqrt(maxJumpHeight * -2.0f * Physics.gravity.y);
    }

    //removes bounciness when moving down slopes, keeps the direction of the movement aligned with the slope angle
    protected Vector3 AdjustVelocityToSlope(Vector3 velocity)
    {
        Quaternion rotation;
        Vector3 adjustedVelocity;
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 2f))
        {
            rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            adjustedVelocity = rotation * velocity;

            //only adjust the _velocity if were moving down a slope which means out Y component of velocity must be negative, otherwise dont change velocity
            if (adjustedVelocity.y < 0)
            {
                return adjustedVelocity;

            }
        }

        if (_isSliding)
        {
            adjustedVelocity = Vector3.ProjectOnPlane(new Vector3(0, _ySpeed, 0), hitNormal);

            velocity = Vector3.zero;

            adjustedVelocity = adjustedVelocity + velocity;

            return adjustedVelocity;
        }

        return velocity;
    }

    protected override void Actions()
    {
        if (_isGrounded && Input.GetButtonDown("SpiritJump"))
        {
            Jump();
            //FMODUnity.RuntimeManager.PlayOneShot("event:/", GetComponent<Transform>().position);
        }

        //get cameras forward and right vectors
        //We zero the y components since we do not want out character to fly
        //multiply input X vector by camera right vector
        //multiply input Z vector by camera forward vector
        //add these two vectors to get the correct rotation
        //Clamp the magnitude so that diagonal movement is not faster

        Vector3 forward = _camera.transform.forward;
        Vector3 right = _camera.transform.right;
        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;

        Vector3 forwardRelativeVerticalInput = forward * playerInput.y;
        Vector3 rightRelativeVerticalInput = right * playerInput.x;

        Vector3 cameraRelativeMovement = forwardRelativeVerticalInput + rightRelativeVerticalInput;

        float magnitude = Mathf.Clamp01(cameraRelativeMovement.magnitude) * _maxSpeed;

        cameraRelativeMovement.Normalize();
        _velocity = cameraRelativeMovement * magnitude;

        _velocity = AdjustVelocityToSlope(_velocity);
        _velocity.y += _ySpeed;


        _controller.Move(_velocity * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
        CheckIfPlayerIsSliding();
    }

    private bool GroundSphereCastGroundCheck()
    {
        if (Physics.SphereCast(_originForGroundSphereCast.position, _radiusSphereGroundCheck, -transform.up, out RaycastHit hit, _maxDistanceForSphereGroundCheck))
        {
            return true;
        }
        else
            return false;
    }

    private void CheckIfPlayerIsSliding()
    {
        float angle = Vector3.Angle(Vector3.up, hitNormal);

        if (!GroundSphereCastGroundCheck() && angle > _controller.slopeLimit && _ySpeed < 0)
        {
            _isSliding = true;
        }
        else
            _isSliding = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_originForGroundSphereCast.position - transform.up * _maxDistanceForSphereGroundCheck, _radiusSphereGroundCheck);
    }

}

