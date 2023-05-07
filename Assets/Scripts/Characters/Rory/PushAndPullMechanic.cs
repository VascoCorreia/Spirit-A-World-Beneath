using UnityEngine;

public class PushAndPullMechanic : MonoBehaviour
{
    public static bool isPulling { get; set; } = false;
    [SerializeField] private float _maxPushingAndPullingSpeed = 2f;
    
    private float _defaultSpeed;
    private RoryMovement _roryMovement;

    private void Awake()
    {
        _roryMovement = GetComponent<RoryMovement>();
    }

    private void Start()
    {
        _defaultSpeed = _roryMovement.maxSpeed;
    }

    private void Update()
    {
        //If character pulling 
        if (isPulling)
        {
            _roryMovement.maxSpeed = _maxPushingAndPullingSpeed;
        }
        
        //if character is not pulling
        if(!isPulling)
        {
            _roryMovement.maxSpeed = _defaultSpeed;
        }
    }
}
