using System;
using UnityEngine;

public class buttonClickedEventArgs : EventArgs
{
    public GameObject associatedDoor;

    public buttonClickedEventArgs(GameObject associatedDoor)
    {
        this.associatedDoor = associatedDoor;
    }
}