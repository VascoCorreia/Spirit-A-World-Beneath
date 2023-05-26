using UnityEngine;
using VLB;

public class CrystalController : SpiritPlayerController
{
    [SerializeField] private float _lightBeamTranslateSpeed;
    [SerializeField] private MeshRenderer _crystalRenderer;

    private Light _spotLight;
    private VolumetricLightBeam _lightBeam;

    protected override void Awake()
    {
        base.Awake();

        _lightBeam = GetComponentInChildren<VolumetricLightBeam>();
        GetSpotLight();
    }
    protected override void Update()
    {
        base.Update();
        GetPlayerInput();
        Actions();
    }

    protected override void Actions()
    {
        _lightBeam.transform.Rotate(playerInput.x * Time.deltaTime * _lightBeamTranslateSpeed, playerInput.y * Time.deltaTime * _lightBeamTranslateSpeed, 0);
    }

    //protected override void GetPlayerInput()
    //{
    //    base.GetPlayerInput();

    //    if (Input.GetButtonDown("SpiritExitPossession"))
    //    {
    //        _spiritPossession.ExitPossession();
    //    }
    //}

    private void GetSpotLight()
    {
        Light[] allLights = GetComponentsInChildren<Light>();

        foreach (Light light in allLights)
        {
            if (light.type == LightType.Spot)
            {
                _spotLight = light;
            }
        }
    }

    public void EnableLights()
    {
        _lightBeam.enabled = true;
        _spotLight.enabled = true;
        _spotLight.color = _crystalRenderer.material.color;
        _lightBeam.color = _crystalRenderer.material.color;
    }

    public void DisableLights()
    {
        _lightBeam.enabled = false;
        _spotLight.enabled = false;
        _spotLight.color = _crystalRenderer.material.color;
        _lightBeam.color = _crystalRenderer.material.color;
    }
}