using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGizmoTests : MonoBehaviour
{

    //Draws a ray in the direction the camera is looking
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 1000);
    }
}
