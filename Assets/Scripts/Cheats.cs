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

    // Start is called before the first frame update
    void Start()
    {
        _invicibility = false;
        _roryCurrentTeleportIndex = 0;
        _spiritCurrentTeleportIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Invicibility") && !_invicibility)
        {
            print(_invicibility);

            _invicibility = true;
        }

        else if (Input.GetButtonDown("Invicibility") && _invicibility)
        {
            _invicibility = false;
        }

        if(Input.GetButtonDown("Human Teleport"))
        {
            TeleportToCheatPosition(_rory, _roryCurrentTeleportIndex);

            if (_roryCurrentTeleportIndex == _teleportCheatPosition.Count - 1)
            {
                _roryCurrentTeleportIndex = 0;
            }

            _roryCurrentTeleportIndex++;
        }

        if (Input.GetButtonDown("Spirit Teleport"))
        {
            TeleportToCheatPosition(_spirit, _spiritCurrentTeleportIndex);

            if (_spiritCurrentTeleportIndex == _teleportCheatPosition.Count - 1)
            {
                _spiritCurrentTeleportIndex = 0;
            }

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
