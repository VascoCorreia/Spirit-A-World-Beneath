using UnityEngine;

public interface IInteractable
{
    public bool isInteracted { get; set; }
    void Interacted(GameObject player);
    void Released(GameObject player);

}