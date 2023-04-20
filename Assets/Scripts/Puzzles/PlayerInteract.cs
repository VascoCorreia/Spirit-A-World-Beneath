using System;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Camera _roryCamera;

    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float interactableDistance;
    private GameObject interactedObject;

    private void OnEnable()
    {
        Death.playerDied += playerHasDiedEventHandler;
    }

    //very important to unsubscribe specially since it is a static event
    private void OnDisable()
    {
        Death.playerDied -= playerHasDiedEventHandler;
    }

    private void Start()
    {
        interactableDistance = 15f;
        _layerMask = LayerMask.GetMask("Interactable");
    }
    private void Update()
    {
        Interaction();
    }
    public void Interact(Camera camera)
    {
        Ray ray = new(camera.transform.position, camera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit info, interactableDistance, _layerMask))
        {
            //cache the interacted object
            interactedObject = info.collider.gameObject;

            //look at interacted object
            transform.LookAt(interactedObject.transform);

            //disable regular character rotating with camera
            GetComponent<CharacterRotation>().enabled = false;

            interactedObject.GetComponent<IInteractable>().Interacted(gameObject);
        }
        else
        {
            return;
        }
    }

    public void StopInteract()
    {
        if (interactedObject != null)
        {
            interactedObject.GetComponent<IInteractable>().Released(gameObject);
            GetComponent<CharacterRotation>().enabled = true;
        }
    }

    private void Interaction()
    {
        if (Input.GetButtonDown("HumanInteract"))
        {
           Interact(_roryCamera);
        }
        if (Input.GetButtonUp("HumanInteract"))
        {
            StopInteract();
        }
    }
    private void playerHasDiedEventHandler()
    {
        enabled = false;
    }
}
