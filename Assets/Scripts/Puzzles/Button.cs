using System;
using UnityEngine;

public class Button : MonoBehaviour, IInteractable
{
    [field: SerializeField] public Barrier associatedBarrier { get; private set; }

    public void Interacted(GameObject player)
    {
        //PlaySound
        //PlayAnimation
        associatedBarrier.OpenBarrier();
    }

    public void Released(GameObject player)
    {
        //PlaySound
        //PlayAnimation
        associatedBarrier.CloseBarrier();
    }
}
