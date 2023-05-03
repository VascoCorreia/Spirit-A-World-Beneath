using UnityEngine;

public class CharacterRotation : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _rotationSpeed;
    private Vector2 _playerInput;

    void Update()
    {
        getPlayerInput();
        Rotate();
    }

    private void Rotate()
    {
        //transform.rotation = Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0);

        Vector3 targetDirection = Vector3.zero;

        targetDirection = _camera.transform.forward *  _playerInput.y;
        targetDirection = targetDirection + _camera.transform.right * _playerInput.x;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
            targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed* Time.deltaTime);

        transform.rotation = playerRotation;
    }

    private void getPlayerInput()
    {
        _playerInput.x = Input.GetAxis("HumanHorizontal");
        _playerInput.y = Input.GetAxis("HumanVertical");
    }
}
