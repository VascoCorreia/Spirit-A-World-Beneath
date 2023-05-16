using System;
using UnityEngine;


//This class is used for Spirit controllers that require translation mainly
public class MovableController : SpiritPlayerController
{
    [field: SerializeField] public PlayerInteract _playerInteract { get; private set; }
    [field: SerializeField] public SpiritPossession _spiritPossession { get; private set; }
    [field: SerializeField] public CharacterController _controller { get; private set; }
    [field: SerializeField, Range(0f, 50f)] public float _maxSpeed { get; set; }

    [SerializeField, Range(0f, 10f)] private float maxJumpHeight;
    [SerializeField] protected Vector3 _velocity;
    [SerializeField] private bool _onGround;
    private float _ySpeed;

    protected override void OnEnable()
    {
        base.OnEnable();
        _controller = GetComponent<CharacterController>();
        _playerInteract = GetComponent<PlayerInteract>();
        _spiritPossession = GameObject.Find("Possession").GetComponent<SpiritPossession>();

        _maxSpeed = 8f;
    }

    protected override void Update()
    {
        base.Update();
        getPlayerInput();

        if (_spiritPossession.typeInPossession == null)
        {
            applyGravity();
            Actions();
        }
    }

    //gets player movement input
    protected override void getPlayerInput()
    {
        base.getPlayerInput();
        //R1
        if (Input.GetButtonDown("SpiritPossession"))
        {
            _spiritPossession.tryPossession();
        }
        //R2
        if (Input.GetButtonDown("SpiritExitPossession"))
        {
            _spiritPossession.ExitPossession();
        }
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

    //https://screenrec.com/share/62pUYiuKDW
    private void Jump()
    {
        _ySpeed += Mathf.Sqrt(maxJumpHeight * -2.0f * Physics.gravity.y);
    }

    //removes bounciness when moving down slopes, keeps the direction of the movement aligned with the slope angle
    protected Vector3 AdjustVelocityToSlope(Vector3 velocity)
    {
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

    protected override void Actions()
    {
        if (_onGround && Input.GetButtonDown("SpiritJump"))
        {
            Jump();
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
        _velocity.y = _ySpeed;
        _velocity = AdjustVelocityToSlope(_velocity);

        _controller.Move(_velocity * Time.deltaTime);
    }
}

