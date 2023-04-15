using UnityEngine;

public class Torch : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    public void Interacted(GameObject player)
    {
        if (player.tag == "Rory")
        {
            transform.SetParent(_playerTransform);
            transform.rotation = new Quaternion(0, 0, 0, 0);
            transform.localPosition = new Vector3(0, 0, 0);
            _rb.useGravity = false;
        }
    }

    public void Released(GameObject player)
    {
        if (player.tag == "Rory")
        {
            _rb.useGravity = true;
            transform.SetParent(null);
        }
    }
}
