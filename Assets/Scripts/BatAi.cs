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
    private HumanPlayerController _player;
    private SpiritPossession _spiritPossession;
    private State _currentState;

    public float startChasingDistance;
    public float rotationSpeed = 5f; //Rotation speed
    public float movementSpeed = 0.5f; //movement speed
    public int currentTarget; //current target, waypoint or player
    public bool isCalled; //current target, waypoint or player
    public Vector3 positionToGoWhenCalled; //current target, waypoint or player



    private void Awake()
    {
        _rory = GameObject.Find("Rory").transform;
        _player = _rory.GetComponent<HumanPlayerController>();
        _spiritPossession = GameObject.Find("Possession").GetComponent<SpiritPossession>();

        _currentState = State.Wander;

        _player.OnWhistleSucessfull += BatCalling;
    }

    private void OnDestroy()
    {
        _player.OnWhistleSucessfull -= BatCalling;
    }

    private void OnEnable()
    {
        isCalled = false;
    }
    void Update()
    {
        Rotate();
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
        //if(!_spiritPossession.alreadyInPossession)
        //{
        //    if (Vector3.Distance(transform.position, _rory.transform.position) > startChasingDistance)
        //        MoveTo(allWaypoints[currentTarget].position);
        //}

        //else if (_spiritPossession.typeInPossession == "Bat" && _spiritPossession.currentPossessedObject != gameObject)
        //{
        //    if (Vector3.Distance(transform.position, _spiritPossession.currentPossessedObject.transform.position) > startChasingDistance)
        //    {
        //        MoveTo(allWaypoints[currentTarget].position);
        //        Debug.Log("1");
        //    }
        //}

        //if (isCalled)
        //{
        //    MoveTo(positionToGoWhenCalled);

        //    if ((Vector3.Distance(transform.position, positionToGoWhenCalled) < 0.5f))
        //    {
        //        isCalled = false;
        //    }
        //    Debug.Log("2");

        //}

        //if (_spiritPossession.typeInPossession == "Bat" && _spiritPossession.currentPossessedObject != gameObject)
        //{
        //    if (Vector3.Distance(transform.position, _spiritPossession.currentPossessedObject.transform.position) <= startChasingDistance)
        //    {
        //        MoveTo(_spiritPossession.currentPossessedObject.transform.position);
        //        isCalled = false;
        //        Debug.Log("3");
        //    }
        //}

        ////Chase Rory if close enough
        //if (Vector3.Distance(transform.position, _rory.transform.position) <= startChasingDistance)
        //{
        //    MoveTo(_rory.transform.position);
        //    isCalled = false;
        //    Debug.Log("4");
        //}
    }

    private void MoveTo(Vector3 positionToMoveTo)
    {
        transform.position = Vector3.MoveTowards(transform.position, positionToMoveTo, movementSpeed * Time.deltaTime);
    }

    void Rotate()
    {
        if ((Vector3.Distance(transform.position, _rory.transform.position) > startChasingDistance) || isCalled)
        {
            transform.LookAt(_rory);
        }

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
        }

        else if (_spiritPossession.typeInPossession == "Bat" && _spiritPossession.currentPossessedObject != gameObject && Vector3.Distance(transform.position, _spiritPossession.currentPossessedObject.transform.position) <= startChasingDistance)
        {
            _currentState = State.ChasePossessedBat;

            if (Vector3.Distance(transform.position, _spiritPossession.currentPossessedObject.transform.position) <= startChasingDistance)
            {
            }
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
            _currentState = State.Wander;

        Debug.Log(_currentState);
    }
}