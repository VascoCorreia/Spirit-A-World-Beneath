using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float interactableDistance;
    [SerializeField] private string[] _objectsThatPlayerCanInteractWith;
    [SerializeField] private float _interactCooldown = 1f;
    [SerializeField] private bool _canInteract;

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
        _canInteract = true;
    }

    private void Update()
    {
        if (gameObject.CompareTag("Spirit"))
        {
            Interaction("Spirit");    
        }

        if (gameObject.CompareTag("Rory"))
        {
            Interaction("Human");
        }
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

                    //Rotate to face object
                    Vector3 direction = interactedObject.transform.position - transform.position;
                    direction.y = 0;
                    Quaternion rotation = Quaternion.LookRotation(direction);
                    transform.rotation = rotation;

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
        }
    }

    private void Interaction(string character)
    {
        if (Input.GetButtonDown(character + "Interact") && _canInteract)
        {
            Interact(_camera);
            StartCoroutine(Cooldowns.Cooldown(_interactCooldown, (flag) => _canInteract = flag));
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
