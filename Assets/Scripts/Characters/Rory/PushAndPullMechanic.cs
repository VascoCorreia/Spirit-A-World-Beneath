using UnityEngine;

public class PushAndPullMechanic : MonoBehaviour
{
    [SerializeField] private float maxPushingAndPullingSpeed = 2f;

    private RoryMovement roryManager;
    private Animator _animator;
    public static bool isPulling { get; set; } = false;

    private int isPullingHash;

    private void Awake()
    {
        roryManager = GetComponent<RoryMovement>();
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        isPullingHash = Animator.StringToHash("IsPulling");
    }
    private void Update()
    {
        //If character starts pulling 
        if (isPulling && _animator.GetBool(isPullingHash).Equals(false))
        {
            roryManager.maxSpeed = maxPushingAndPullingSpeed;
            _animator.SetBool(isPullingHash, true);
        }
        
        //if character is not pulling
        if(!isPulling)
        {
            roryManager.maxSpeed = roryManager.maxSpeed;
            _animator.SetBool(isPullingHash, false);
        }
    }
}
