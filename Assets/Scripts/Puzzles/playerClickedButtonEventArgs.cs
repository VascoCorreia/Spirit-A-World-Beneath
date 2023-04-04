using System;
using UnityEngine;

public class playerClickedButtonEventArgs : EventArgs
{
    public GameObject interactable;

    public playerClickedButtonEventArgs(GameObject interactable)
    {
        this.interactable = interactable;
    }
}