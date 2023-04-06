using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BatAi : MonoBehaviour
{
    public enum WanderType { Random, Waypoint }; //the bat can wander randomlly, or follow the waypoints

    public WanderType wanderType;
    public float wanderRadius = 7f;
    public Transform[] waypoints;
    public float walkingSpeed = 6;  //walking speed
    public float runningSpeed = 9;  //runnning speed (for when its chasing Rory)
    public NavMeshAgent agent;

    private Vector3 wanderPoint; //waypoints
    private int waypointIndex = 0;

    public float startWaitingTimer = 4; //  Wait time of every action
    public float rotationTime = 2;  //  Wait time when the bat detect near the player without seeing
    public float viewRadius = 15;   //  Radius of the bat view
    public float viewAngle = 90;    //  Angle of the bat view
    public LayerMask playerMask;
    public LayerMask obstacleMask;

    Vector3 playerLastPosition = Vector3.zero;  //  Last position of the player when he was near the bat
    Vector3 m_PlayerPosition;   //  Last position of the player when the player is seen by the enemy

    float m_WaitingTime;    //  Variable of the wait time that makes the delay
    float m_rotationTime;   //  Variable of the wait time to rotate when the player is near that makes the delay
    bool m_playerInRange;   //  If the player is in range of vision, state of Chasing the player
    bool m_PlayerNear;  //  If the player is near
    bool m_Wandering;   //  If the bat is wandering, then state of wandering
    bool m_CaughtPlayer;    //player was caught

    void Start()
    {
        agent.speed = walkingSpeed;
        wanderPoint = RandomWanderPoint();

        m_PlayerPosition = Vector3.zero;
        m_Wandering = true;
        m_CaughtPlayer = false;
        m_playerInRange = false;
        m_PlayerNear = false;
        m_WaitingTime = startWaitingTimer;
        m_rotationTime = rotationTime;

        m_CurrentWaypointIndex = 0;
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.speed = walkingSpeed;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }

    void Update()
    {
        if (!m_Wandering)
        {
            ChasingPlayer();
        }
        else
        {
            Wander();
        }
    }

    public void Wander()
    {
        //Random Wandering
        if(wanderType == WanderType.Random)
        {
            if(Vector3.Distance(transform.position, wanderPoint) < 2f)
            {
                wanderPoint = RandomWanderPoint();
            }
            else
            {
                agent.SetDestination(wanderPoint);
            }
        }
        else
        {
            //waypoint wandering
            if (Vector3.Distance(waypoints[waypointIndex].position, transform.position) < 2f)
            {
                if (waypointIndex == waypoints.Length - 1)
                {
                    waypointIndex = 0;
                }
                else
                {
                    waypointIndex++;
                }
            }
            else
            {
                agent.SetDestination(waypoints[waypointIndex].position);
            }
        }

        //if player gets near
        if (m_PlayerNear)
        {
            //Byy checking if the player is near, the bat will go to that position
            if (m_rotationTime <= 0)
            {
                Move(walkingSpeed);
                LookingPlayer(playerLastPosition);
            }
        }
        else
        {
            m_PlayerNear = false;
            playerLastPosition = Vector3.zero;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);

            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                //If the bat arrives to the waypoint position then wait for a moment and go to the next
                if (m_WaitingTime <= 0)
                {
                    NextPoint();
                    Move(walkingSpeed);
                    m_WaitingTime = startWaitingTimer;
                }
                else
                {
                    Stop();
                    m_WaitingTime -= Time.deltaTime;
                }
            }
        }

    }

    public void ChasingPlayer()
    {
        //if rory calls bats fuction
            //CallingBats();

        //  The enemy is chasing the player
        m_PlayerNear = false;
        playerLastPosition = Vector3.zero;

        if (!m_CaughtPlayer)
        {
            Move(runningSpeed);
            navMeshAgent.SetDestination(m_PlayerPosition);
        }

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (m_WaitingTime <= 0 && !m_CaughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
            {
                //  Check if the bat is not near to the player, returns to wandering after the wait time delay
                m_Wandering = true;
                m_PlayerNear = false;
                Move(walkingSpeed);
                m_rotationTime = rotationTime;
                m_WaitingTime = startWaitingTimer;
                agent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            }
        }
    }

    public void AttackRory()
    {
        //reload the scene when it attacks Rory

        m_CaughtPlayer = true;
    }

    public Vector3 RandomWanderPoint()
    {
        Move(walkingSpeed);
        Vector3 randomPoint = (Random.insideUnitSphere * wanderRadius) + transform.position;
        UnityEngine.AI.NavMeshHit navHit;
        UnityEngine.AI.NavMesh.SamplePosition(randomPoint, out navHit, wanderRadius, -1);
        return new Vector3(navHit.position.x, transform.position.y, navHit.position.z);
    }

    void Move(float speed)
    {
        agent.speed = speed;
    }
}
