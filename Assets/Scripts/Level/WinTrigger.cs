using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask _winTriggerPlayerLayerMask;
    
    private BoxCollider _collider;
    private Collider[] _numberOfPlayersInsideTheWinTrigger;
    [SerializeField] private bool _roryInside;
    [SerializeField] private bool _spiritInside;
    void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _spiritInside = false;
        _roryInside = false;
    }

    void Update()
    {
        _numberOfPlayersInsideTheWinTrigger = Physics.OverlapBox(transform.position, _collider.bounds.extents, Quaternion.identity, _winTriggerPlayerLayerMask);

        foreach(Collider collider in _numberOfPlayersInsideTheWinTrigger)
        {
            if(collider.tag == "Spirit")
            {
                _spiritInside = true;
            }
            if (collider.tag == "Rory")
            {
                _roryInside = true;
            }
        }
        if (_roryInside && _spiritInside)
        {
            SceneManager.LoadScene("Main menu");
        }
    }
}
