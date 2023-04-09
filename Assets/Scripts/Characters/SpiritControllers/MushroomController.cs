using UnityEngine;

public class MushroomController : SpiritPlayerController
{
    [field: SerializeField] public SpiritPossession _spiritPossession { get; private set; }

    [SerializeField, Range(3, 10)] private float _maxSize;
    [SerializeField] private Vector3 _minSize;
    [SerializeField] private float _mushroomGrowthSpeed;

    void Start()
    {
        _minSize = transform.localScale;
        _mushroomGrowthSpeed = Random.Range(0.005f, 0.05f);
        _maxSize = Random.Range(3f, 10f);
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
        transform.localScale += new Vector3(_playerInput.y * _mushroomGrowthSpeed, _playerInput.y * _mushroomGrowthSpeed, _playerInput.y * _mushroomGrowthSpeed);
        sizeConstraints();
        Debug.Log(_playerInput.y);
    }

    public void sizeConstraints()
    {
        if (transform.localScale.x > _maxSize && transform.localScale.y > _maxSize && transform.localScale.z > _maxSize)
        {
            transform.localScale = new Vector3(_maxSize, _maxSize, _maxSize);
        }

        if (transform.localScale.x < _minSize.x && transform.localScale.y < _minSize.y && transform.localScale.z < _minSize.z)
        {
            transform.localScale = _minSize;
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
}
