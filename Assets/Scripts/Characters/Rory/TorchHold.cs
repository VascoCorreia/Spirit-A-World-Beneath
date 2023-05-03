using UnityEngine;

public class TorchHold : MonoBehaviour
{
    public GameObject Torch;
    public Transform PlayerTransform;
    public float range = 3f;
    public float Go = 100f;
    public Camera Camera;

    void Update()
    {
        if (Input.GetButtonDown("HumanInteract"))
        {
            StartPickUp();
        }

        if (Input.GetButtonUp("HumanInteract"))
        {
            Drop();
        }
    }

    void StartPickUp()
    {
        RaycastHit hit;
        if(Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();
            if(target  != null)
            {
                Pickup();
            }
        }
    }

    void Pickup()
    {
        Torch.transform.SetParent(PlayerTransform);
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    void Drop()
    {
        PlayerTransform.DetachChildren();
    }
}