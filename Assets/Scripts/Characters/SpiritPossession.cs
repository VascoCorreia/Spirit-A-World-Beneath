using Cinemachine;
using System;
using System.Collections;
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
    [field: SerializeField] public GameObject _currentPossessedObject { get; private set; }

    [SerializeField] private float _possessionCooldown;
    [SerializeField] private GameObject _cacheSpirit;

    private LayerMask _combinedPossessionLayerMask;
    private LayerMask _possessionStaticLayerMask;
    private LayerMask _possessionDynamicLayerMask;

    public TypeInPossession typeInPossession;

    public Action possessionFailed;
    public Action exitPossession;
    public event Action<possessionEventArgs> possessionSucessfull;

    private void Awake()
    {
        _spiritCamera = GameObject.Find("SpiritCameraBrain").GetComponent<Camera>();
        _spiritFreeLookCamera = GameObject.Find("SpiritCamera").GetComponent<CinemachineFreeLook>();
        _cacheSpirit = GameObject.Find("Spirit");

        _possessionDynamicLayerMask = LayerMask.GetMask("PossessableDynamic");
        _possessionStaticLayerMask = LayerMask.GetMask("PossessableStatic");
        _combinedPossessionLayerMask = (_possessionDynamicLayerMask | _possessionStaticLayerMask);
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

            //Reactivate the spirit object
            _cacheSpirit.SetActive(true);

            //Update the type of possession to No Possession
            typeInPossession = TypeInPossession.None;

            //Teleport the spirit to the position of the possessd object
            _cacheSpirit.transform.position = _currentPossessedObject.transform.position;

            //Run the Exit possession function on the possessed object 
            _currentPossessedObject.GetComponent<IPossessable>().ExitPossess();

            //Invoke event
            exitPossession?.Invoke();

        }
    }
    //When clicking R1
    //casts an array from the camera so that the direction is correctly aligned with the crosshair
    //if collides with a possessable enemy invokes event and calls Possess method from interface, that sends as an argument the gameObject that was hit
    //if does not collide send an event to UI to spawn text on screen
    //deactivates the gameObject
    public void tryPossession()
    {
        if (_canPossess)
        {
            Ray ray = new(_spiritCamera.transform.position, _spiritCamera.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit info, _possessableDistance, _combinedPossessionLayerMask) && _canPossess && !_alreadyInPossession)
            {
                _alreadyInPossession = true;

                //cache the possessed object
                _currentPossessedObject = info.collider.gameObject;

                //Update the type of possession to the current posssessed object
                switch (info.collider.tag)
                {
                    case "Bat":
                        typeInPossession = TypeInPossession.Bat;
                        break;
                    case "Mushroom":
                        typeInPossession = TypeInPossession.Mushroom;
                        break;

                }

                //Run the Possess function in the possessed object
                _currentPossessedObject.GetComponent<IPossessable>().Possess(new possessionEventArgs(info.collider.gameObject));

                //Invoke possession successfull event and pass the possessed object
                possessionSucessfull?.Invoke(new possessionEventArgs(info.collider.gameObject));

                //Deactivate Spirit Object
                _cacheSpirit.SetActive(false);

                //Start Cooldown
                StartCoroutine(Cooldown(_possessionCooldown, (i) =>
                {
                    _canPossess = i;
                }));
            }

            else
            {
                //Invoke failed event if possession fails
                possessionFailed?.Invoke();

                //Start the cooldown
                StartCoroutine(Cooldown(_possessionCooldown, (i) =>
                {
                    _canPossess = i;
                }));
            }
        }
    }
}
