using UnityEngine;

public class BatAi : MonoBehaviour
{

    public Transform[] allWaypoints; //array of all waypoints
    private Transform Player;
    private Player pixa;

    private float distance;
    public float playerNear;
    public float rotationSpeed = 5f; //Rotation speed
    public float movementSpeed = 0.5f; //movement speed
    public int currentTarget; //current target, waypoint or player
    public bool isCalled; //current target, waypoint or player
    public Transform positionToGoWhenCalled; //current target, waypoint or player

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        pixa = Player.GetComponent<Player>();
        pixa.OnWhistle += BatCalling;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnEnable()
    {
        isCalled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Movement();
        ChangeTarget();
        //BatCalling();
    }

    void Movement()
    {
        if (isCalled)
        {
            transform.position = Vector3.MoveTowards(transform.position, positionToGoWhenCalled.position, 0.01f);

            if ((Vector3.Distance(transform.position, positionToGoWhenCalled.position) < 0.5f))
            {
                isCalled = false;
            }
        }

        else if (Vector3.Distance(transform.position, Player.transform.position) <= playerNear)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, 0.01f);
            isCalled = false;
        }
        else if (Vector3.Distance(transform.position, Player.transform.position) > playerNear)
        {
            transform.position = Vector3.MoveTowards(transform.position, allWaypoints[currentTarget].position, 0.01f);
        }
    }

    //void Movement()
    //{

    //    transform.position = Vector3.MoveTowards(transform.position, currentTargetTEST.transform.position, 0.01f);

    //}

    void Rotate()
    {
        if ((Vector3.Distance(transform.position, Player.transform.position) > playerNear) || isCalled)
        {
            transform.LookAt(Player);
        }

        transform.LookAt(allWaypoints[currentTarget].position);
    }

    //void Rotate()
    //{

    //    transform.LookAt(currentTargetTEST.transform.position);

    //}

    void ChangeTarget()
    {
        if (transform.position == allWaypoints[currentTarget].position)
        {
            currentTarget++;
            currentTarget = UnityEngine.Random.Range(0, allWaypoints.Length);
        }
    }

    //void ChangeTarget(GameObject test)
    //{
    //    currentTargetTEST = test;
    //}

    public void BatCalling(calledBatsEventArgs calledBats)
    {
        if (calledBats.getCalledBat() == gameObject)
        {
            Debug.Log(calledBats);
            isCalled = true;
            positionToGoWhenCalled = calledBats.getPosition();
        }
    }
}