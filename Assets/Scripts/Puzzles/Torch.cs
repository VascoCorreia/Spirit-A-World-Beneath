using UnityEngine;

public class Torch : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Rigidbody _rb;

    public bool isInteracted { get; set; }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    public void Interacted(GameObject player)
    {
        isInteracted = true;

        transform.SetParent(_playerTransform);
        transform.rotation = new Quaternion(0, 0, 0, 0);
        transform.localPosition = new Vector3(0, 0, 0);
        _rb.useGravity = false;

    }

    public void Released(GameObject player)
    {
        if (isInteracted)
        {
            _rb.useGravity = true;
            transform.SetParent(null);
        }
    }
}
