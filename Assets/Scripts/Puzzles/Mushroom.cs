using UnityEngine;
using UnityEngine.Windows;

public class Mushroom : MonoBehaviour, IPossessable
{
    [SerializeField, Range(0, 10)] private float _maxSize;
    [SerializeField] private float _minSize;

    void Start()
    {
        _minSize = transform.localScale.x;
    }

    private void Update()
    {
        sizeConstraints();
    }
    public void ExitPossess()
    {
        Destroy(GetComponent<CharacterController>());
        Destroy(GetComponent<SpiritPlayerController>());
    }

    public void Possess(possessionEventArgs raycastedMushroom)
    {
        if (raycastedMushroom.getPossessedEnemy() == gameObject)
        {
            gameObject.AddComponent<CharacterController>();
            gameObject.AddComponent<SpiritPlayerController>();
        }
    }

    void sizeConstraints()
    {
        if (transform.localScale.x < _minSize && transform.localScale.y < _minSize && transform.localScale.z < _minSize)
        {
            return;
        }

        if (transform.localScale.x > _maxSize && transform.localScale.y > _maxSize && transform.localScale.z > _maxSize)
        {
            transform.localScale = new Vector3(_maxSize, _maxSize, _maxSize);
        }

        if (transform.localScale.x < _minSize && transform.localScale.y < _minSize && transform.localScale.z < _minSize)
        {
            transform.localScale = new Vector3(_minSize, _minSize, _minSize);
        }
    }
}
