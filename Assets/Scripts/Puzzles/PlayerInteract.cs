using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float interactableDistance;
    [SerializeField] private string[] _objectsThatPlayerCanInteractWith;

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
        if (gameObject.CompareTag("Spirit"))
            Interaction("Spirit");

        if (gameObject.CompareTag("Rory"))
            Interaction("Human");
    }
    public void Interact(Camera camera)
    {
        Ray ray = new(camera.transform.position, camera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit info, interactableDistance, _layerMask))
        {
            //Only execute interaction if the player can interact with object
            foreach (string tag in _objectsThatPlayerCanInteractWith)
            {
                if (info.collider.gameObject.CompareTag(tag))
                {

                    //cache the interacted object
                    interactedObject = info.collider.gameObject;
                    Vector3 direction = interactedObject.transform.position - transform.position;
                    direction.y = 0;
                    Quaternion rotation = Quaternion.LookRotation(direction);
                    transform.rotation = rotation;

                    //transform.LookAt(direction);
                    //cannot be like this because character rotates up
                    //look at interacted object
                    //transform.LookAt(interactedObject.transform, Vector3.up);
                    //transform.rotation = new Quaternion(0, transform.rotation.y, transform.rotation.z, 0);

                    //disable regular character rotating with camera
                    GetComponent<CharacterRotation>().enabled = false;

                    //Make stuff happen
                    interactedObject.GetComponent<IInteractable>().Interacted(gameObject);
                }
            }
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

    private void Interaction(string character)
    {
        if (Input.GetButtonDown(character + "Interact"))
        {
            Interact(_camera);
        }
        if (Input.GetButtonUp(character + "Interact"))
        {
            StopInteract();
        }
    }
    private void playerHasDiedEventHandler()
    {
        enabled = false;
    }
}
