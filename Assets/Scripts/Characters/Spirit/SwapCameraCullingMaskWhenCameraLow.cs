using Cinemachine;
using UnityEngine;

public class SwapCameraCullingMaskWhenCameraLow : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook _freeLookCamera;
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _cameraCloseToPlayerLayerMask;
    [SerializeField] private LayerMask _normalCameraLayerMask;

    private void Start()
    {
        _normalCameraLayerMask = _camera.cullingMask;
    }

    void Update()
    {
        if(_freeLookCamera != null)
        {
            if(_freeLookCamera.transform.position.y <= -0.2f && _camera.cullingMask != LayerMask.NameToLayer("_cameraCloseToPlayerLayerMask"))
            {
                _camera.cullingMask = _cameraCloseToPlayerLayerMask;
            }
            
            if(_freeLookCamera.transform.position.y > -0.2f && _camera.cullingMask != _normalCameraLayerMask)
            {
                _camera.cullingMask = _normalCameraLayerMask;
            }
        }
    }
}
