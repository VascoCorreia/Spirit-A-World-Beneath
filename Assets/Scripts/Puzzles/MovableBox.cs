using UnityEngine;

public class MovableBox : MonoBehaviour, IInteractable
{
    [SerializeField] private float _distanceToObstacleThreshold;
    public bool isInteracted { get; set; } = false;
    private GameObject cachedPlayer = null;

    #region Helps fixing an issue where after the player stops pushing the CC and Rigidbody collide and the box gets propelled since CC mass is infinite
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Rory") || collision.collider.CompareTag("Spirit"))
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Rory") || collision.collider.CompareTag("Rory"))
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }
    #endregion

    private void Update()
    {
        //If player is not grounded stop pushing
        if (cachedPlayer != null && !cachedPlayer.GetComponent<RoryMovement>().isGrounded)
        {
            PushAndPullMechanic.isPulling = false;
            transform.SetParent(null, true);
        }
    }

    public void Interacted(GameObject player)
    {
        isInteracted = true;

        //The player can only use mechanic when grounded
        if (player.GetComponent<RoryMovement>().isGrounded)
        {
            PushAndPullMechanic.isPulling = true;
            FMODUnity.RuntimeManager.PlayOneShot("event:/Others/Dragging Box", GetComponent<Transform>().position);

            //disable regular character rotating with camera 
            player.GetComponent<CharacterRotation>().enabled = false;

            //cache player
            cachedPlayer = player;

            //parent object to player to simulate pushing and pulling
            transform.SetParent(player.transform, true);
        }
    }

    public void Released(GameObject player)
    {
        if (isInteracted)
        {
            isInteracted = false;
            PushAndPullMechanic.isPulling = false;
            player.GetComponent<CharacterRotation>().enabled = true;
            transform.SetParent(null, true);
            cachedPlayer = null;
        }
    }
}
