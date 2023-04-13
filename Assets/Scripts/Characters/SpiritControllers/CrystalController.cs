using UnityEngine;
using VLB;

public class CrystalController : SpiritPlayerController
{
    [field: SerializeField] public SpiritPossession _spiritPossession { get; private set; }

    private Light _spotLight;
    private VolumetricLightBeam _lightBeam;

    private void Awake()
    {
        _lightBeam = GetComponentInChildren<VolumetricLightBeam>();
        _spiritPossession = GameObject.Find("Possession").GetComponent<SpiritPossession>();
        GetSpotLight();

    }
    protected override void OnEnable()
    {
        base.OnEnable();

        EnableLights();
    }

    private void OnDisable()
    {
        _lightBeam.enabled = false;
    }

    protected override void Update()
    {
        base.Update();
        getPlayerInput();
        Actions();
    }

    protected override void Actions()
    {
        //if (_pointLight.intensity > _maxIntensity)
        //{
        //    _pointLight.intensity = _maxIntensity;
        //}

        //_pointLight.intensity += (_playerInput.y * _lightIntensityGrowthSpeed);

        _lightBeam.transform.Rotate(-playerInput.y, playerInput.x, 0);
    }

    protected override void getPlayerInput()
    {
        base.getPlayerInput();

        if (Input.GetButtonDown("SpiritExitPossession"))
        {
            _spiritPossession.ExitPossession();
        }
    }

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

    private void EnableLights()
    {
        _lightBeam.enabled = true;
        _spotLight.enabled = true;
        _spotLight.color = GetComponent<MeshRenderer>().material.color;
        _lightBeam.color = GetComponent<MeshRenderer>().material.color;
    }
}