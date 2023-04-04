using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private BoxCollider _collider;
    [SerializeField] private LayerMask _winTriggerPlayerLayerMask;

    private int _numberOfPlayersInsideTheWinTrigger;

    void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        _numberOfPlayersInsideTheWinTrigger = Physics.OverlapBox(transform.position, _collider.bounds.extents, Quaternion.identity, _winTriggerPlayerLayerMask).Length;

        if (_numberOfPlayersInsideTheWinTrigger > 0)
        {
            ScenesManager.Instance.RestartScene();
        }

    }
}
