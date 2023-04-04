using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour, IInteractable
{
    public void Interacted()
    {
        Debug.Log("I am gay");
    }

    public void Released()
    {
        Debug.Log("I am not gay");

    }
}
