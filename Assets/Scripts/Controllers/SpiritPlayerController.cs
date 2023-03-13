using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Use arrows and numpad 0 and 1 to move spirit
public class SpiritPlayerController : MonoBehaviour
{
    [SerializeField] private float _hoverHeight;
    [SerializeField] private Vector3 _velocity;
    [SerializeField] private Camera _spiritCamera;
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
        calculateVelocityAndMove();
    }

    private void LateUpdate()
    {
        applyGravity();
    }

    //gets player movement input
    private void getPlayerMovementInput()
    {
        _playerInput.x = Input.GetAxis("SpiritHorizontal");
        _playerInput.y = Input.GetAxis("SpiritVertical");
    }

    //This function is reponsible for continuously applying gravity to our human.
    private void applyGravity()
    {
        _ySpeed += Physics.gravity.y * Time.deltaTime;

        //if we set it to zero the _controller.isGrounded property does not work as intended so we must set it to a small negative value
        if (isGrounded())
        {
            _ySpeed = 0f;
        }
    }

    //this function calculates the velocity magnitude and direction of the current movement and after makes the character move.
    //It is also responsible for listening to jumping input
    private void calculateVelocityAndMove()
    {
        if (isGrounded() && Input.GetButtonDown("SpiritJump"))
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
        _velocity = AdjustvelocityToSlope(_velocity);

        _controller.Move(_velocity * Time.deltaTime);
    }

    //https://screenrec.com/share/62pUYiuKDW
    public void Jump()
    {
        _ySpeed += Mathf.Sqrt(maxJumpHeight * -2.0f * Physics.gravity.y);
    }

    //removes bounciness when moving down slopes, keeps the direction of the movement align with the slope angle
    private Vector3 AdjustvelocityToSlope(Vector3 velocity)
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

    private bool isGrounded()
    {
        Debug.DrawRay(transform.position, Vector3.down, Color.green);

        if(Physics.Raycast(transform.position, Vector3.down, _hoverHeight))
        {
            return true;
        }

        return false;
    }
}
