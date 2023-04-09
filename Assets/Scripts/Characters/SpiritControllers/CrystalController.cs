using UnityEngine;

public class CrystalController : SpiritPlayerController
{
    private Light _pointLight;
    [SerializeField] private float _lightIntensityGrowthSpeed;
    [SerializeField] private float _lightMaxRange;
    [field: SerializeField] public SpiritPossession _spiritPossession { get; private set; }


    private float _maxIntensity;

    private void Awake()
    {
        _pointLight = GetComponentInChildren<Light>();
    }
    void Start()
    {
        _lightIntensityGrowthSpeed = Random.Range(0.005f, 0.01f);
        _pointLight.color = GetComponent<MeshRenderer>().material.color;
        _maxIntensity = 2f;
        _pointLight.range = _lightMaxRange;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _spiritPossession = GameObject.Find("Possession").GetComponent<SpiritPossession>();
    }

    protected override void Update()
    {
        base.Update();
        getPlayerInput();
        Actions();
    }

    protected override void Actions()
    {
        if (_pointLight.intensity > _maxIntensity)
        {
            _pointLight.intensity = _maxIntensity;
        }

        _pointLight.intensity += (_playerInput.y * _lightIntensityGrowthSpeed);
    }

    protected override void getPlayerInput()
    {
        base.getPlayerInput();

        if (Input.GetButtonDown("SpiritExitPossession"))
        {
            _spiritPossession.ExitPossession();
        }
    }
}