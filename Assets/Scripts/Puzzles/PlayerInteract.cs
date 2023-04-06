using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [field: SerializeField] public float interactableDistance { get; private set; }

    private GameObject _interactedObject;

    private void Start()
    {
        interactableDistance = 15f;
        _layerMask = LayerMask.GetMask("Interactable");
    }

    public void Interact(Camera camera)
    {
        Ray ray = new(camera.transform.position, camera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit info, interactableDistance, _layerMask))
        {
            _interactedObject = info.collider.gameObject;

            _interactedObject.GetComponent<IInteractable>().Interacted();
        }
        else
        {
            return;
        }
    }

    public void StopInteract()
    {
        if (_interactedObject != null)
        {
            _interactedObject.GetComponent<IInteractable>().Released();
        }
    }
}
