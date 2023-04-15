using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

public class SpiritPossession : MonoBehaviour
{
    [field: SerializeField] public bool alreadyInPossession { get; set; }
    [field: SerializeField] public bool canPossess { get; set; }
    [field: SerializeField] public Camera spiritCamera { get; private set; }
    [field: SerializeField] public CinemachineFreeLook spiritFreeLookCamera { get; private set; }
    [field: SerializeField, Range(10f, 30f)] public float possessableDistance { get; private set; }
    [field: SerializeField] public GameObject currentPossessedObject { get; private set; }

    [SerializeField] private float _possessionCooldown;
    [SerializeField] private GameObject _cacheSpirit;

    private LayerMask _combinedPossessionLayerMask;
    private LayerMask _possessionStaticLayerMask;
    private LayerMask _possessionDynamicLayerMask;

    public string typeInPossession;

    public Action possessionFailed;
    public Action exitPossession;
    public event Action<possessionEventArgs> possessionSucessfull;

    private void Awake()
    {
        spiritCamera = GameObject.Find("SpiritCameraBrain").GetComponent<Camera>();
        spiritFreeLookCamera = GameObject.Find("SpiritCamera").GetComponent<CinemachineFreeLook>();
        _cacheSpirit = GameObject.Find("Spirit");

        _possessionDynamicLayerMask = LayerMask.GetMask("PossessableDynamic");
        _possessionStaticLayerMask = LayerMask.GetMask("PossessableStatic");
        _combinedPossessionLayerMask = (_possessionDynamicLayerMask | _possessionStaticLayerMask);
    }

    private void OnEnable()
    {
        typeInPossession = null;
        alreadyInPossession = false;
        canPossess = true;
        _possessionCooldown = 2f;
    }

    public void ExitPossession()
    {
        if (alreadyInPossession)
        {
            alreadyInPossession = false;

            //Teleport the spirit to the position of the possessd object
            _cacheSpirit.transform.position = currentPossessedObject.transform.position;

            //Reactivate the spirit object
            _cacheSpirit.SetActive(true);

            //Update the type of possession to No Possession
            typeInPossession = null;

            //Run the Exit possession function on the possessed object 
            currentPossessedObject.GetComponent<IPossessable>().ExitPossess();

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
        if (canPossess)
        {
            Ray ray = new(spiritCamera.transform.position, spiritCamera.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit info, possessableDistance, _combinedPossessionLayerMask) && canPossess && !alreadyInPossession)
            {
                alreadyInPossession = true;

                IPossessable possessable = info.collider.GetComponent<IPossessable>();
                //cache the possessed object
                currentPossessedObject = info.collider.gameObject;

                typeInPossession = possessable.TypeInPossession;

                //Run the Possess function in the possessed object
                possessable.Possess(new possessionEventArgs(info.collider.gameObject));

                //Invoke possession successfull event and pass the possessed object
                possessionSucessfull?.Invoke(new possessionEventArgs(info.collider.gameObject));

                //Deactivate Spirit Object
                _cacheSpirit.SetActive(false);

                //Start Cooldown
                StartCoroutine(Cooldowns.Cooldown(_possessionCooldown, (i) =>
                {
                    canPossess = i;
                }));
            }

            else
            {
                //Invoke failed event if possession fails
                possessionFailed?.Invoke();

                //Start the cooldown
                StartCoroutine(Cooldowns.Cooldown(_possessionCooldown, (i) =>
                {
                    canPossess = i;
                }));
            }
        }
    }
}
