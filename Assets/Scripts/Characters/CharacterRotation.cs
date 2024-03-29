using UnityEngine;

public class CharacterRotation : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private string _character;
    private Vector2 _playerInput;

    private void Start()
    {
        _rotationSpeed = 10f;
    }

    private void Awake()
    {
        
    }

    void Update()
    {
        getPlayerInput();
        Rotate();
    }

    private void Rotate()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = _camera.transform.forward * _playerInput.y;
        targetDirection = targetDirection + _camera.transform.right * _playerInput.x;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
            targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    private void getPlayerInput()
    {
        if(_character == "Rory")
        {
            _playerInput.x = Input.GetAxis("HumanHorizontal");
            _playerInput.y = Input.GetAxis("HumanVertical");
        }
        if(_character == "Spirit")
        {
            _playerInput.x = Input.GetAxis("SpiritHorizontal");
            _playerInput.y = Input.GetAxis("SpiritVertical");
        }
    }
}
