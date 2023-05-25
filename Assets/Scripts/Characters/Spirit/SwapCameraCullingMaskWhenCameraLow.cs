using Cinemachine;
using UnityEngine;

public class SwapCameraCullingMaskWhenCameraLow : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook _freeLookCamera;
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _cameraCloseToPlayerLayerMask;
    [SerializeField] private LayerMask _normalCameraLayerMask;
    [SerializeField] private float _switchCullingMaskYValyeThreshold;

    private void Start()
    {
        _normalCameraLayerMask = _camera.cullingMask;
    }

    void Update()
    {
        if(_freeLookCamera != null)
        {
            if(_freeLookCamera.m_YAxis.Value <= _switchCullingMaskYValyeThreshold && _camera.cullingMask != LayerMask.NameToLayer("_cameraCloseToPlayerLayerMask"))
            {
                _camera.cullingMask = _cameraCloseToPlayerLayerMask;
                
            }
            
            if(_freeLookCamera.m_YAxis.Value > _switchCullingMaskYValyeThreshold && _camera.cullingMask != _normalCameraLayerMask)
            {
                _camera.cullingMask = _normalCameraLayerMask;
            }
        }
    }
}
