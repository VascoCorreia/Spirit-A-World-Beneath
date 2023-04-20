using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotation : MonoBehaviour
{
    [SerializeField] private Camera _humanCamera;

    void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        transform.rotation = Quaternion.Euler(0, _humanCamera.transform.eulerAngles.y, 0);
    }
}
