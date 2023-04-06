using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public float startWaitingTimer = 4;             //  Wait time of every action
    public float rotationTime = 2;                  //  Wait time when the enemy detect near the player without seeing
    public float walkingSpeed = 6;                  //  Walking speed, speed in the nav mesh agent
    public float runningSpeed = 9;                  //  Running speed
    public float viewRadius = 15;                   //  Radius of the enemy view
    public float viewAngle = 90;                    //  Angle of the enemy view
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float meshResolution = 1.0f;
    public int edgeIterations = 4;
    public float edgeDistance = 0.5f;

    public Transform[] waypoints;                   //  All the waypoints where the enemy patrols
    int m_CurrentWaypointIndex;                     //  Current waypoint where the enemy is going to

    Vector3 playerLastPosition = Vector3.zero;      //  Last position of the player when was near the enemy
    Vector3 m_PlayerPosition;                       //  Last position of the player when the player is seen by the enemy

    float m_WaitingTime;                            //  Variable of the wait time that makes the delay
    float m_rotationTime;                           //  Variable of the wait time to rotate when the player is near that makes the delay
    bool m_playerInRange;                           //  If the player is in range of vision, state of chasing
    bool m_PlayerNear;                              //  If the player is near, state of hearing
    bool m_Wandering;                                //  If the enemy is patrol, state of patroling
    bool m_CaughtPlayer;                            //  if the enemy has caught the player

    void Start()
    {
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

    private void Update()
    {
        EnviromentView();

        if (!m_Wandering)
        {
            Chasing();
        }
        else
        {
            Patroling();
        }
    }

    private void Patroling()
    {
        if (m_PlayerNear)
        {
            //  Check if the enemy detect near the player, so the enemy will move to that position
            if (m_rotationTime <= 0)
            {
                Move(walkingSpeed);
                LookingPlayer(playerLastPosition);
            }
            else
            {
                //  The enemy wait for a moment and then go to the last player position
                m_rotationTime -= Time.deltaTime;
            }
        }
        else
        {
            m_PlayerNear = false;
            playerLastPosition = Vector3.zero;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                //  If the enemy arrives to the waypoint position then wait for a moment and go to the next
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

    public void NextPoint()
    {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }


    void Move(float speed)
    {
        navMeshAgent.speed = speed;
    }

    void CaughtPlayer()
    {
        m_CaughtPlayer = true;
    }

    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);
        
        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (m_WaitingTime <= 0)
            {
                m_PlayerNear = false;
                Move(walkingSpeed);
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                m_WaitingTime = startWaitingTimer;
                m_rotationTime = rotationTime;
            }
            else
            {
                m_WaitingTime -= Time.deltaTime;
            }
        }
    }

    void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);
                
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    m_playerInRange = true;
                    m_Wandering = false;
                }
                else
                {
                    //If the player is behind a obstacle the player position will not be registered
                    m_playerInRange = false;
                }
            }
            
            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {
                //If the player is further than the view radius, then the enemy will no longer keep the player's current position.
                //Or the enemy is a safe zone, the enemy will no chase
                m_playerInRange = false;
            }
            
            if (m_playerInRange)
            {
               //If the enemy no longer sees the player, then the enemy will go to the last position that has been registered
                m_PlayerPosition = player.transform.position;
            }
        }
    }
}