using System;
using UnityEngine;

//The character as of now is 1.56m long in real world units or 0.78 in Unity capsule units. (normal height for 13 year old)
public class RoryManager : MonoBehaviour
{
    [field: SerializeField] public CharacterController _characterController { get; private set; }
    [field: SerializeField] public RoryAnimatorManager _animatorManager { get; private set; }
    [field: SerializeField] public RoryMovement _movement { get; private set; }

    private Vector2 _playerInput;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animatorManager = GetComponent<RoryAnimatorManager>();

        //ensures scripts are enabled on level restart/change
        _characterController.enabled = true;
    }

    //subsribe to death event
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
        getPlayerInput();
        #region Animation

        _animatorManager.UpdateAnimatorMovementValues(_playerInput.x, _playerInput.y);

        #endregion
        #region Movement

        _movement.HandleMovement(_playerInput);

        #endregion
    }

    //gets player movement input
    private void getPlayerInput()
    {
        _playerInput.x = Input.GetAxis("HumanHorizontal");
        _playerInput.y = Input.GetAxis("HumanVertical");
    }

    
    void playerHasDiedEventHandler()
    {
        _characterController.enabled = false;
    }
}
