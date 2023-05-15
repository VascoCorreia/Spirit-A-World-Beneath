using System.Collections.Generic;
using UnityEngine;

public class MushroomController : SpiritPlayerController
{
    [field: SerializeField] public SpiritPossession _spiritPossession { get; private set; }

    [SerializeField, Range(4, 6)] private float _maxSize;
    [SerializeField] private Vector3 _minSizeVector;
    [SerializeField] private Vector3 _maxSizeVector;
    [SerializeField] private float _mushroomGrowthSpeed; //randomized
    [SerializeField, Range(0,1)] private float _growthPercentageYToStartGrowingX; 

    Dictionary<string, float> _growthPercentages = new Dictionary<string, float>();

    void Start()
    {
        _minSizeVector = transform.localScale;
        _mushroomGrowthSpeed = Random.Range(0.01f, 0.05f);
        _maxSizeVector = new Vector3(_maxSize, _maxSize, _maxSize);
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
        _growthPercentages = CalculateGrowthPercentages();

        if (_growthPercentages["X"] < 1 - _growthPercentageYToStartGrowingX && _growthPercentages["Y"] > _growthPercentageYToStartGrowingX)
        {
            transform.localScale += new Vector3(playerInput.y * _mushroomGrowthSpeed, playerInput.y * _mushroomGrowthSpeed, playerInput.y * _mushroomGrowthSpeed);
        }
        else if (_growthPercentages["X"] < 1 - _growthPercentageYToStartGrowingX && _growthPercentages["Y"] != 1)
        {
            transform.localScale += new Vector3(0, playerInput.y * _mushroomGrowthSpeed, 0);
        }

        else if (_growthPercentages["X"] > 1 - _growthPercentageYToStartGrowingX && _growthPercentages["Y"] == 1)
        {
            transform.localScale += new Vector3(playerInput.y * _mushroomGrowthSpeed, 0, playerInput.y * _mushroomGrowthSpeed);
        }

        sizeConstraints();
    }

    //This function restricts the mushroom for growing or shrinking indefinetely
    public void sizeConstraints()
    {

        if (transform.localScale.x > _maxSizeVector.x)
        {
            transform.localScale = new Vector3(_maxSize, transform.localScale.y, transform.localScale.z);
        }

        if (transform.localScale.y > _maxSizeVector.y)
        {
            transform.localScale = new Vector3(transform.localScale.x, _maxSize, transform.localScale.z);
        }

        if (transform.localScale.z > _maxSizeVector.z)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, _maxSize);
        }

        if (transform.localScale.x < _minSizeVector.x)
        {
            transform.localScale = new Vector3(_minSizeVector.x, transform.localScale.y, transform.localScale.z);
        }

        if (transform.localScale.y < _minSizeVector.y)
        {
            transform.localScale = new Vector3(transform.localScale.x, _minSizeVector.y, transform.localScale.z);
        }

        if (transform.localScale.z < _minSizeVector.z)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, _minSizeVector.z);
        }

    }

    protected override void getPlayerInput()
    {
        base.getPlayerInput();

        if (Input.GetButtonDown("SpiritExitPossession"))
        {
            _spiritPossession.ExitPossession();
        }
    }

    private Dictionary<string, float> CalculateGrowthPercentages()
    {
        Dictionary<string, float> result = new Dictionary<string, float>();

        float range = _maxSize - _minSizeVector.y;
        float correctedStartValueY = transform.localScale.y - _minSizeVector.y;
        float percentageY = correctedStartValueY / range;

        float correctedStartValueX = transform.localScale.x - _minSizeVector.x;
        float percentageX = correctedStartValueX / range;

        result.Add("X", percentageX);
        result.Add("Y", percentageY);

        return result;
    }
}
