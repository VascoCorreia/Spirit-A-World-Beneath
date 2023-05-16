using UnityEngine;

public class BatAi : MonoBehaviour
{
    private enum State
    {
        Wander,
        ChaseRory,
        ChasePossessedBat,
        Called
    }

    public Transform[] allWaypoints; //array of all waypoints
    private Transform _rory;
    private WhistleMechanic _roryWhistle;
    private SpiritPossession _spiritPossession;
    [SerializeField] private State _currentState;

    public float startChasingDistance;
    public float rotationSpeed = 5f; //Rotation speed
    public float movementSpeed; //movement speed
    public int currentTarget; //current target, waypoint or player
    public bool isCalled; //current target, waypoint or player
    public Vector3 positionToGoWhenCalled; //current target, waypoint or player

    private void Awake()
    {
        _rory = GameObject.Find("Rory").transform;
        _roryWhistle = _rory.GetComponent<WhistleMechanic>();
        _spiritPossession = GameObject.Find("Possession").GetComponent<SpiritPossession>();

        _currentState = State.Wander;
    }

    private void OnEnable()
    {
        _roryWhistle.OnWhistleSucessfull += BatCalling;
        isCalled = false;

    }
    private void OnDisable()
    {
        _roryWhistle.OnWhistleSucessfull -= BatCalling;

    }
    void Update()
    {
        //Rotate();
        StateTransitions();
        Movement();
        ChangeWaypoint();
    }

    //Priority:
    //1 - Near Rory
    //2 - Near Spirit
    //3 - Rory Whistle
    //4 - Wander
    void Movement()
    {
        switch (_currentState)
        {
            case State.Wander:
                MoveTo(allWaypoints[currentTarget].position);
                break;
            case State.ChaseRory:
                MoveTo(_rory.transform.position);
                break;
            case State.ChasePossessedBat:
                MoveTo(_spiritPossession.currentPossessedObject.transform.position);
                break;
            case State.Called:
                MoveTo(positionToGoWhenCalled);
                break;

        }
    }
    private void MoveTo(Vector3 positionToMoveTo)
    {
        transform.position = Vector3.MoveTowards(transform.position, positionToMoveTo, movementSpeed * Time.deltaTime);
        transform.LookAt(positionToMoveTo);
    }

    void Rotate()
    {
        if (Vector3.Distance(transform.position, _rory.transform.position) < startChasingDistance)
        {
            transform.LookAt(_rory);
        }

        else if (isCalled)
        {
            transform.LookAt(positionToGoWhenCalled);
        }
        else
            transform.LookAt(allWaypoints[currentTarget].position);
    }
    void ChangeWaypoint()
    {
        if (transform.position == allWaypoints[currentTarget].position)
        {
            currentTarget++;
            currentTarget = Random.Range(0, allWaypoints.Length);
        }
    }

    //Event handler
    public void BatCalling(WhistleEventArgs calledBats)
    {
        if (calledBats.getCalledBat() == gameObject)
        {
            isCalled = true;
            positionToGoWhenCalled = calledBats.getPlayerPositionWhenWhistled();
        }
    }

    private void StateTransitions()
    {
        if (Vector3.Distance(transform.position, _rory.transform.position) <= startChasingDistance)
        {
            _currentState = State.ChaseRory;
            isCalled = false;
        }

        else if (
                _spiritPossession.typeInPossession == "Bat"
                 && _spiritPossession.currentPossessedObject != gameObject
                 && Vector3.Distance(transform.position, _spiritPossession.currentPossessedObject.transform.position) <= startChasingDistance)
        {
            _currentState = State.ChasePossessedBat;
            isCalled = false;
        }

        else if (isCalled)
        {
            _currentState = State.Called;

            if ((Vector3.Distance(transform.position, positionToGoWhenCalled) < 0.5f))
            {
                isCalled = false;
            }
        }

        else
        {
            _currentState = State.Wander;
            isCalled = false;
        }
    }
}