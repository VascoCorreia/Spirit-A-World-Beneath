using Cinemachine;
using System;
using UnityEngine;

public class SpiritPlayerController : MonoBehaviour
{
    [field: SerializeField] public Camera _spiritCamera { get; private set; }
    [field: SerializeField] public CinemachineFreeLook _spiritFreeLookCamera { get; private set; }
    [field: SerializeField] public PlayerInteract _playerInteract { get; private set; }
    [field: SerializeField] public SpiritPossession _spiritPossession { get; private set; }
    [field: SerializeField] public CharacterController _controller { get; private set; }
    [field: SerializeField, Range(0f, 50f)] public float _maxSpeed { get; set; }

    [SerializeField, Range(0f, 10f)] private float maxJumpHeight;
    [SerializeField] private float _mushroomGrowthSpeed;
    [SerializeField] private Vector3 _velocity;
    [SerializeField] private bool _onGround;
    private Vector2 _playerInput;
    private float _ySpeed;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _playerInteract = GetComponent<PlayerInteract>();
        _spiritCamera = GameObject.Find("SpiritCameraBrain").GetComponent<Camera>();
        _spiritFreeLookCamera = GameObject.Find("SpiritCamera").GetComponent<CinemachineFreeLook>();
        _spiritPossession = GameObject.Find("TestPossession").GetComponent<SpiritPossession>();
    }

    private void Start()
    {
        _maxSpeed = 10;
        _mushroomGrowthSpeed = UnityEngine.Random.Range(0.005f, 0.05f);
    }

    void Update()
    {
        getPlayerInput();
        applyGravity();

        switch (_spiritPossession.typeInPossession)
        {
            case SpiritPossession.TypeInPossession.Bat:
                possessedBatPlayerMovement();
                break;
            case SpiritPossession.TypeInPossession.Mushroom:
                possessedMushromPlayerMovement();
                break;
            case SpiritPossession.TypeInPossession.None:
                SpiritMovement();
                break;
        }
    }

    //gets player movement input
    private void getPlayerInput()
    {
        _playerInput.x = Input.GetAxis("SpiritHorizontal");
        _playerInput.y = Input.GetAxis("SpiritVertical");

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
            _playerInteract.Interact(_spiritCamera);
        }
        if (Input.GetButtonUp("SpiritInteract"))
        {
            _playerInteract.StopInteract();
        }
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

    //https://screenrec.com/share/62pUYiuKDW
    private void Jump()
    {
        _ySpeed += Mathf.Sqrt(maxJumpHeight * -2.0f * Physics.gravity.y);
    }

    //removes bounciness when moving down slopes, keeps the direction of the movement align with the slope angle
    private Vector3 AdjustVelocityToSlope(Vector3 velocity)
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

    private void SpiritMovement()
    {
        _maxSpeed = 10f;
        if (_onGround && Input.GetButtonDown("SpiritJump"))
        {
            Jump();
        }
        //get player input
        //get cameras forward and right vectors
        //multiply input X vector by camera right vector
        //multiply input Z vector by camera forward vector
        //add these two vectors

        Vector3 forward = _spiritCamera.transform.forward;
        Vector3 right = _spiritCamera.transform.right;
        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;

        Vector3 forwardRelativeVerticalInput = forward * _playerInput.y;
        Vector3 rightRelativeVerticalInput = right * _playerInput.x;

        Vector3 cameraRelativeMovement = forwardRelativeVerticalInput + rightRelativeVerticalInput;

        float magnitudeTest = Mathf.Clamp01(cameraRelativeMovement.magnitude) * _maxSpeed;

        cameraRelativeMovement.Normalize();
        _velocity = cameraRelativeMovement * magnitudeTest;
        _velocity.y = _ySpeed;
        _velocity = AdjustVelocityToSlope(_velocity);

        _controller.Move(_velocity * Time.deltaTime);
    }

    void possessedBatPlayerMovement()
    {
        _maxSpeed = 15f;
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

    private void possessedMushromPlayerMovement()
    {
        transform.localScale += new Vector3(_playerInput.y * _mushroomGrowthSpeed, _playerInput.y * _mushroomGrowthSpeed, _playerInput.y * _mushroomGrowthSpeed);
    }
}

