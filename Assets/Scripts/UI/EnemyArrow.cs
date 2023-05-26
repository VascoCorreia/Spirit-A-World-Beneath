using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    [SerializeField] private float _distancePlayerBatToActivate;
    [SerializeField] private LayerMask _batLayerMask;

    private Collider[] _allBats;
    private GameObject _closestBat;
    private Transform _player;
    private MeshRenderer _meshRenderer;


    void Start()
    {
        //_allBats = GameObject.FindGameObjectsWithTag("Bat");
        _player = GetComponent<Transform>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        GetClosestBat();
        ActivateWhenBatClose();

        if (_closestBat != null)
            transform.LookAt(_closestBat.transform);
    }

    private void GetClosestBat()
    {
        _allBats = Physics.OverlapSphere(transform.position, _distancePlayerBatToActivate, _batLayerMask);

        for (int i = 0; i < _allBats.Length - 1; i++)
        {
            if (Vector3.Distance(_allBats[i].transform.position, transform.parent.position) > Vector3.Distance(_allBats[i + 1].transform.position, transform.parent.position))
            {
                _closestBat = _allBats[i + 1].gameObject;
            }
        }
    }

    private void ActivateWhenBatClose()
    {
        if (_allBats.Length > 0)
        {
            _meshRenderer.enabled = true;
        }
        else
        {
            _meshRenderer.enabled = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _distancePlayerBatToActivate);
    }
}
