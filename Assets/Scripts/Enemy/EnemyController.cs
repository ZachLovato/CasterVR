using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] bool isdummy = false;
    private float timer = 0;
    enum STATE
    {
        
        IDLE,
        PATROL,
        WANDER,
        CHASE,
        ATTACK,
        DODGE,
        RETREAT,
		Dummey
	}

    [SerializeField] private STATE state;
    [SerializeField] private NavMeshAgent agent;

	#region State Variables
	[Header("IDLE state")]
    [SerializeField] private float idleDuration;
    [SerializeField] private STATE IdleToState;

	[Header("Patrol state")]
	[SerializeField] private GameObject[] patrolLocations;
    [SerializeField] private float minDistance = 1;
	[SerializeField] private STATE PatrolToState;
	[SerializeField] private bool chooseRandomPatrol = false;
    private int patrolNum = 0;

    [Header("Wander State")]
    [SerializeField, Range(3,20)] private float wanderRadius = 5;
	[SerializeField] private STATE WanderToState;
	private Vector3 wanderSpot = Vector3.zero;

    [Header("Chase State")]
	[SerializeField] private float minChaseDistance = 6;


	[Space, SerializeField] private STATE FoundPlayerState;
	#endregion

	[SerializeField] private EnemyVision vision;
	[SerializeField] private EnemyAttacks attack;
    private GameObject playerObject;

	[Space, SerializeField] bool isDegugging = false;
	[SerializeField] bool isDrawingGizmo = false;

    private bool doOnce = true;
    private Vector3 pos;


    public bool isDisable = false;

    // Start is called before the first frame update
    void Start()
    {
        state = STATE.IDLE;
        agent = GetComponent<NavMeshAgent>();
        attack = GetComponent<EnemyAttacks>();
        attack.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //agent.destination = patrolLocations[patrolNum].transform.position;
        if (isdummy) state = STATE.Dummey;

        switch (state)
        {
            case STATE.Dummey: break;
            case STATE.IDLE: IdleState(); break;
            case STATE.PATROL: PatrolState(); break;
            case STATE.WANDER: WanderState(); break;
            case STATE.CHASE: ChaseState(); break;
			case STATE.ATTACK: AttackState(); break;
			case STATE.RETREAT: 
                break;

        }


        printDebug("Current State:" + state);
    }

	#region Helper Functions

    public void FoundPlayer(GameObject player)
    {
        playerObject = player;
        state = FoundPlayerState;
    }
    public bool isPlayerInLOS()
    {
        if (vision != null) printDebug("vision is here");
        else printDebug("Vision is null");
        bool isSeen = vision.isPlayerSeen();
        if (isSeen)
        {
            playerObject = vision.getPlayer();
        }
        return isSeen;
    }

    public void setNewTargetPosition(Vector3 newPos)
    {
		pos = newPos;
		if (doOnce)
        {
			
            doOnce = false;
		}
        
    }

	private bool checkTimer(float duration)
    {
        if (timer >= duration)
        {
            timer = 0;
            return true;
        }
        else
        {
            return false;
        }
    }

    private float getDistanceFromPatorl()
    {
        return Vector3.Distance(transform.position, patrolLocations[patrolNum].transform.position);

    }
    private int getRandomPatrol()
    {
        int patrolCount = patrolLocations.Length;
        return Random.Range(0, patrolCount);
    }

    
	#endregion

	#region states

	private void IdleState()
    {
		//if the timer is greator then 
		if (checkTimer(idleDuration))
		{
			state = IdleToState;
            timer = 0;
		}
        else
        {
            timer += Time.deltaTime;
        }

        if (isPlayerInLOS()) state = STATE.CHASE;
	}

    private void PatrolState()
    {
		if (patrolLocations.Count() <= 0)
		{
			state = STATE.IDLE;
			printDebug("There are no patrol locations");
            return;
		}

        if (getDistanceFromPatorl() < minDistance)
        {
            if (chooseRandomPatrol) patrolNum = getRandomPatrol();
            else
            {
                patrolNum = (patrolNum + 1) % patrolLocations.Length;
            }
            state = PatrolToState;
		}
        else
        {
		    agent.destination = patrolLocations[patrolNum].transform.position;
        }

		if (isPlayerInLOS()) state = STATE.CHASE;
	}

    private void WanderState()
    {
        if (wanderSpot == Vector3.zero)
        {
            wanderSpot = transform.position + (Random.insideUnitSphere * wanderRadius);
			NavMeshHit hit;
			NavMesh.SamplePosition(wanderSpot, out hit, wanderRadius, 1);
			wanderSpot = hit.position;
		}

        float distance = Vector3.Distance(transform.position, wanderSpot);


		if (distance <= minDistance)
        {
            state = WanderToState;
            wanderSpot = Vector3.zero;
        }
        else
        {
            agent.destination = wanderSpot;
        }

		if (isPlayerInLOS()) state = STATE.CHASE;
	}

    private void ChaseState()
    {
        if (Vector3.Distance(transform.position, playerObject.transform.position) <= minChaseDistance)
        {
            state = STATE.ATTACK;
        }
        else
        {
            agent.destination = playerObject.transform.position;
        }


    }

    private void AttackState()
    {
        attack.enabled = true;
        if (pos != null) agent.SetDestination(pos);
    }


	#endregion


	#region Debug
	private void OnDrawGizmos()
	{
        if (isDrawingGizmo)
        {
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, wanderRadius);

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(wanderSpot, 1);
		}
        
	}
	#endregion

	private void printDebug(string msg)
    {
        //if (isDegugging) { Debug.Log(msg); }
    }
}
