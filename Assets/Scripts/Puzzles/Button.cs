using System;
using UnityEngine;

public class Button : MonoBehaviour, IInteractable
{
    [field: SerializeField] public Barrier associatedBarrier { get; private set; }

    public void Interacted()
    {
        //PlaySound
        //PlayAnimation
        associatedBarrier.OpenBarrier();
    }

    public void Released()
    {
        //PlaySound
        //PlayAnimation
        associatedBarrier.CloseBarrier();
    }
}
