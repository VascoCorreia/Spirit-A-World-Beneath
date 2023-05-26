using UnityEngine;

public class BatController : SpiritPlayerController
{
    [field: SerializeField] public PlayerInteract _playerInteract { get; private set; }

    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private Vector3 _velocity;

    protected override void Awake()
    {
        base.Awake();
        _playerInteract = GetComponent<PlayerInteract>();
    }
    void Start()
    {
        _maxSpeed = 12f;
        _rotationSpeed = 5f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        GetPlayerInput();
        Rotate();
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
        //_velocity = AdjustVelocityToSlope(_velocity);

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
}

