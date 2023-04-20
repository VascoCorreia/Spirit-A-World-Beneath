using System;
using System.Collections.Generic;
using UnityEngine;

//The character as of now is 1.56m long in real world units or 0.78 in Unity capsule units. (normal height for 13 year old)
public class HumanPlayerController : MonoBehaviour
{
    [field: SerializeField] public Camera _humanCamera { get; private set; }
    [field: SerializeField] public PlayerInteract _playerInteract { get; private set; }
    [field: SerializeField] public CharacterController _controller { get; private set; }
    [field: SerializeField, Range(0f, 50f)] public float maxSpeed { get; set; }

    [SerializeField] private Vector3 _velocity;
    [SerializeField] private bool _onGround;

    [SerializeField, Range(0f, 10f)] private float _maxJumpHeight = 2f;

    private Vector2 _playerInput;
    private float _ySpeed;


    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _playerInteract = GetComponent<PlayerInteract>();

        //ensures scripts are enabled on level restart/change
        _controller.enabled = true;
    }

    private void OnEnable()
    {
        Death.playerDied += playerHasDiedEventHandler;
    }

    //very important to unsubscribe specially since it is a static event
    private void OnDisable()
    {
        Death.playerDied -= playerHasDiedEventHandler;
    }
    void Update()
    {
        getPlayerMovementInput();
        applyGravity();
        calculateVelocityAndMove();
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
        if (_onGround && Input.GetButtonDown("HumanJump") && !PushAndPullMechanic.isPulling)
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

        float magnitudeTest = Mathf.Clamp01(cameraRelativeMovement.magnitude) * maxSpeed;

        cameraRelativeMovement.Normalize();
        _velocity = cameraRelativeMovement * magnitudeTest;

        _velocity.y = _ySpeed;
        _velocity = AdjustvelocityToSlope(_velocity);

        _controller.Move(_velocity * Time.deltaTime);
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
            if (Vector3.Angle(Vector3.up, hitInfo.normal) > _controller.slopeLimit)
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

    void playerHasDiedEventHandler()
    {
        _controller.enabled = false;
    }
}
