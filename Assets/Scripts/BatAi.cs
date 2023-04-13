using UnityEngine;

public class BatAi : MonoBehaviour
{

    public Transform[] allWaypoints; //array of all waypoints
    private Transform _rory;
    private HumanPlayerController _player;

    private float distance;
    public float playerNear;
    public float rotationSpeed = 5f; //Rotation speed
    public float movementSpeed = 0.5f; //movement speed
    public int currentTarget; //current target, waypoint or player
    public bool isCalled; //current target, waypoint or player
    public Vector3 positionToGoWhenCalled; //current target, waypoint or player

    private void Awake()
    {
        _rory = GameObject.Find("Rory").transform;
        _player = _rory.GetComponent<HumanPlayerController>();
        _player.OnWhistleSucessfull += BatCalling;
    }

    private void OnEnable()
    {
        isCalled = false;
    }
    void Update()
    {
        Rotate();
        Movement();
        ChangeTarget();
    }

    void Movement()
    {
        if (isCalled)
        {
            transform.position = Vector3.MoveTowards(transform.position, positionToGoWhenCalled, movementSpeed * Time.deltaTime);

            if ((Vector3.Distance(transform.position, positionToGoWhenCalled) < 0.5f))
            {
                isCalled = false;
            }
        }

        else if (Vector3.Distance(transform.position, _rory.transform.position) <= playerNear)
        {
            transform.position = Vector3.MoveTowards(transform.position, _rory.transform.position, movementSpeed * Time.deltaTime);
            isCalled = false;
        }
        else if (Vector3.Distance(transform.position, _rory.transform.position) > playerNear)
        {
            transform.position = Vector3.MoveTowards(transform.position, allWaypoints[currentTarget].position, movementSpeed * Time.deltaTime);
        }
    }

    void Rotate()
    {
        if ((Vector3.Distance(transform.position, _rory.transform.position) > playerNear) || isCalled)
        {
            transform.LookAt(_rory);
        }

        transform.LookAt(allWaypoints[currentTarget].position);
    }
    void ChangeTarget()
    {
        if (transform.position == allWaypoints[currentTarget].position)
        {
            currentTarget++;
            currentTarget = Random.Range(0, allWaypoints.Length);
        }
    }

    public void BatCalling(WhistleEventArgs calledBats)
    {
        if (calledBats.getCalledBat() == gameObject)
        {
            isCalled = true;
            positionToGoWhenCalled = calledBats.getPlayerPositionWhenWhistled();
        }
    }
}