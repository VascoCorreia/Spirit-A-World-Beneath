using UnityEngine;

public class RoryAnimatorManager : MonoBehaviour
{
    private Animator _animator;
    private CharacterController _characterController;
    private RoryMovement _roryMovement;

    private int isPullingHash;
    private int playerInputXYCombinedHash;
    private int ySpeedHash;
    private int groundedHash;
    private int died;
    private int playerInputYHash;
    private int pushButtonHash;
    private int isFallingHash;
    private int whistledTrigger;

    private Vector2 _playerInput;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        _roryMovement = GetComponent<RoryMovement>();
    }

    private void Start()
    {
        isPullingHash = Animator.StringToHash("isPulling");
        playerInputXYCombinedHash = Animator.StringToHash("playerInputXYCombined");
        ySpeedHash = Animator.StringToHash("ySpeed");
        groundedHash = Animator.StringToHash("grounded");
        died = Animator.StringToHash("died");
        playerInputYHash = Animator.StringToHash("playerInputY");
        pushButtonHash = Animator.StringToHash("pushButton");
        isFallingHash = Animator.StringToHash("isFalling");
        whistledTrigger = Animator.StringToHash("whistled");

    }

    private void OnEnable()
    {
        Death.playerDied += OnDeathAnimation;
    }

    private void OnDisable()
    {
        Death.playerDied -= OnDeathAnimation;
    }

    private void Update()
    {
        getPlayerInput();
        UpdateAnimatorMovementValues(_playerInput.x, _playerInput.y, Mathf.Clamp(RoryMovement._ySpeedTest, -0.2f, 0.2f));
        IsPullingAnimation(_playerInput.y);
    }

    public void UpdateAnimatorMovementValues(float horizontalInput, float verticalInput, float ySpeed)
    {
        float snappedVertical;
        float snappedHorizontal;

        #region SnappedHorizontal
        if (horizontalInput > 0 && horizontalInput < 0.55f)
        {
            snappedHorizontal = 0.5f;
        }
        else if (horizontalInput > 0.55f)
        {
            snappedHorizontal = 1;
        }
        else if (horizontalInput < 0 && horizontalInput > -0.55f)
        {
            snappedHorizontal = -0.5f;
        }
        else if (horizontalInput < -0.55f)
        {
            snappedHorizontal = -1f;
        }
        else
        {
            snappedHorizontal = 0;
        }
        #endregion
        #region SnappedVertical
        if (verticalInput > 0 && verticalInput < 0.55f)
        {
            snappedVertical = 0.5f;
        }
        else if (verticalInput > 0.55f)
        {
            snappedVertical = 1;
        }
        else if (verticalInput < 0 && verticalInput > -0.55f)
        {
            snappedVertical = -0.5f;
        }
        else if (verticalInput < -0.55f)
        {
            snappedVertical = -1f;
        }
        else
        {
            snappedVertical = 0;
        }
        #endregion

        //No matter in which direction the player is moving we always want the same walking or running animation
        float sumOfMovementInputs = Mathf.Abs(snappedHorizontal) + Mathf.Abs(snappedVertical);

        _animator.SetFloat(playerInputXYCombinedHash, Mathf.Clamp01(Mathf.Abs(sumOfMovementInputs)), 0.1f, Time.deltaTime);
        _animator.SetFloat(ySpeedHash, ySpeed, 0.1f, Time.deltaTime);

        if (_roryMovement.isGrounded)
        {
            _animator.SetBool(groundedHash, true);
        }
        if (!_roryMovement.isGrounded)
        {
            _animator.SetBool(groundedHash, false);
        }
    }

    public void IsPullingAnimation(float verticalInput)
    {
        _animator.SetFloat(playerInputYHash, verticalInput);

        if (PushAndPullMechanic.isPulling && _animator.GetBool(isPullingHash).Equals(false))
        {
            _animator.SetBool(isPullingHash, true);
        }

        //if character is not pulling
        if (!PushAndPullMechanic.isPulling && _animator.GetBool(isPullingHash).Equals(true))
        {
            _animator.SetBool(isPullingHash, false);
        }
    }

    private void OnDeathAnimation()
    {
        _animator.SetTrigger(died);
    }

    public void PushingButtonAnimation()
    {
        _animator.SetBool(pushButtonHash, true);
    }

    public void StopPushingButtonAnimation()
    {
        _animator.SetBool(pushButtonHash, false);
    }

    public void PushingButtonStartHandler()
    {
        GetComponent<CharacterRotation>().enabled = false;
        GetComponent<RoryMovement>().enabled = false;
    }

    public void PushingButtonEndHandler()
    {
        GetComponent<CharacterRotation>().enabled = true;
        GetComponent<RoryMovement>().enabled = true;
    }

    public void RoryWhistleAnimation()
    {
        _animator.SetTrigger(whistledTrigger);
    }

    private void getPlayerInput()
    {
        _playerInput.x = Input.GetAxis("HumanHorizontal");
        _playerInput.y = Input.GetAxis("HumanVertical");
    }
}
