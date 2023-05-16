using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    [field: SerializeField] public static bool _invicibility { get; private set; }
    [SerializeField] private GameObject _rory;
    [SerializeField] private GameObject _spirit;
    [SerializeField] List<Transform> _teleportCheatPosition;
    [SerializeField] private int _roryCurrentTeleportIndex;
    [SerializeField] private int _spiritCurrentTeleportIndex;

    void Start()
    {
        _invicibility = false;
        _roryCurrentTeleportIndex = 0;
        _spiritCurrentTeleportIndex = 0;
    }

    void Update()
    {
        if (Input.GetButtonDown("Invicibility") && !_invicibility)
        {
            _invicibility = true;
        }

        else if (Input.GetButtonDown("Invicibility") && _invicibility)
        {
            _invicibility = false;
        }

        if(Input.GetButtonDown("Human Teleport"))
        {

            if (_roryCurrentTeleportIndex == _teleportCheatPosition.Count)
            {
                _roryCurrentTeleportIndex = 0;
            }

            TeleportToCheatPosition(_rory, _roryCurrentTeleportIndex);
            _roryCurrentTeleportIndex++;
        }

        if (Input.GetButtonDown("Spirit Teleport"))
        {
            if (_spiritCurrentTeleportIndex == _teleportCheatPosition.Count)
            {
                _spiritCurrentTeleportIndex = 0;
            }

            TeleportToCheatPosition(_spirit, _spiritCurrentTeleportIndex);
            _spiritCurrentTeleportIndex++;
        }
    }
    
    private void TeleportToCheatPosition(GameObject playerToTeleport, int index)
    {
        playerToTeleport.GetComponentInChildren<CharacterController>().enabled = false;
        playerToTeleport.transform.position = _teleportCheatPosition[index].position;
        playerToTeleport.GetComponentInChildren<CharacterController>().enabled = true;
    }
}
