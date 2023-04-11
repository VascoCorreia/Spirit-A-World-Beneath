using UnityEngine;

public interface IInteractable
{
    void Interacted(GameObject player);
    void Released(GameObject player);

}