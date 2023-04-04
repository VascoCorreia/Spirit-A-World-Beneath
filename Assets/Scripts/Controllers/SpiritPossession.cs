using Cinemachine;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class SpiritPossession : MonoBehaviour
{
    public enum TypeInPossession
    {
        None,
        Bat,
        Mushroom,
    }
    [field: SerializeField] public bool _alreadyInPossession { get; set; }
    [field: SerializeField] public bool _canPossess { get; set; }
    [field: SerializeField] public Camera _spiritCamera { get; private set; }
    [field: SerializeField] public CinemachineFreeLook _spiritFreeLookCamera { get; private set; }
    [field: SerializeField, Range(10f, 30f)] public float _possessableDistance { get; private set; }

    [SerializeField] private LayerMask _possessionLayerMask;
    [SerializeField] private float _possessionCooldown;
    [SerializeField] private GameObject _cacheSpirit;
    [SerializeField] private GameObject _currentPossessedObject;

    public TypeInPossession typeInPossession;

    public Action possessionFailed;
    public Action exitPossession;
    public event Action<possessionEventArgs> possessionSucessfull;

    private void Awake()
    {
        _spiritCamera = GameObject.Find("SpiritCameraBrain").GetComponent<Camera>();
        _spiritFreeLookCamera = GameObject.Find("SpiritCamera").GetComponent<CinemachineFreeLook>();
        _cacheSpirit = GameObject.Find("Spirit");
    }

    private void OnEnable()
    {
        _alreadyInPossession = false;
        _canPossess = true;
        _possessionCooldown = 2f;
    }

    private IEnumerator Cooldown(float cooldownTime, Action<bool> callback)
    {
        callback(false);
        yield return new WaitForSeconds(cooldownTime);
        callback(true);
    }

    public void ExitPossession()
    {
        if (_alreadyInPossession)
        {
            _alreadyInPossession = false;
            _cacheSpirit.SetActive(true);
            typeInPossession = TypeInPossession.None;
            _cacheSpirit.transform.position = _currentPossessedObject.transform.position;
            _currentPossessedObject.GetComponent<IPossessable>().ExitPossess();
            exitPossession?.Invoke();

        }
    }
    //When clicking R1
    //casts an array from the camera so that the direction is correctly aligned with the crosshair
    //if collides with a possessable enemy invokes event and calls Possess method from interface, that sends as an argument the gameObject that was hit
    //if does not collide send an event to UI to spawn text on screen
    //deactivates the gameObjectd
    public void tryPossession()
    {
        if (_canPossess)
        {
            Ray ray = new(_spiritCamera.transform.position, _spiritCamera.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit info, _possessableDistance, _possessionLayerMask) && _canPossess && !_alreadyInPossession)
            {
                _alreadyInPossession = true;

                _currentPossessedObject = info.collider.gameObject;

                _currentPossessedObject.GetComponent<IPossessable>().Possess(new possessionEventArgs(info.collider.gameObject));

                switch (info.collider.tag)
                {
                    case "Bat":
                        typeInPossession = TypeInPossession.Bat;
                        break;
                    case "Mushroom":
                        typeInPossession = TypeInPossession.Mushroom;
                        break;

                }

                possessionSucessfull?.Invoke(new possessionEventArgs(info.collider.gameObject));

                _cacheSpirit.SetActive(false);
                StartCoroutine(Cooldown(_possessionCooldown, (i) =>
                {
                    _canPossess = i;
                }));
            }

            else
            {
                possessionFailed?.Invoke();

                StartCoroutine(Cooldown(_possessionCooldown, (i) =>
                {
                    _canPossess = i;
                }));
            }
        }
    }
}
