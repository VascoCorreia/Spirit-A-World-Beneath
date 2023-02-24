using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Use arrows and numpad 0 and 1 to move spirit
public class SpiritPlayerController : MonoBehaviour
{
    [SerializeField] private Vector3 _velocity;
    [SerializeField, Range(0f, 100f)] private float _maxSpeed = 10f;
    private CharacterController _controller;
    private Vector3 _playerInput;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        getPlayerMovementInput();
        calculateVelocityAndMove();
    }

    private void getPlayerMovementInput()
    {
        _playerInput.x = Input.GetAxis("SpiritHorizontal");
        _playerInput.y = Input.GetAxis("SpiritJump");
        _playerInput.z = Input.GetAxis("SpiritVertical");
    }

    private void calculateVelocityAndMove()
    {

        Vector3 movementDirection = new Vector3(_playerInput.x, _playerInput.y, _playerInput.z);

        //clamping the magnitude to 1 prevents the character from moving faster diagonally.
        float magnitude = Mathf.Clamp01(movementDirection.magnitude) * _maxSpeed;

        //after calculating the magnitude we can normalize the movement direction vector to get the direction of movement
        movementDirection.Normalize();

        _velocity = movementDirection * magnitude;

        _controller.Move(_velocity * Time.deltaTime);
    }
}
