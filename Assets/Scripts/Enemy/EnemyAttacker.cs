using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttacker : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private Health health;

    [Space]
    [SerializeField] private NavMeshAgent agent;
    private Transform player;

    [Header("Movement Settings")]
    [Tooltip("The distance ")]
    [SerializeField] private float MaxDistanceFromPlayer;
    [SerializeField] private float minCircleDistance;
    [SerializeField] private float maxCircleDistance;
    private Vector3 walkTarget;
    [SerializeField] private float minStopDistance;

    [Header("Spell Settings")]
    [SerializeField] private float minSpellAttackDelay;
    [SerializeField] private float maxSpellAttackDelay;
    [SerializeField] private float currentAttackDelay;
    [SerializeField] private float attackTimer;
    [Space, Tooltip("1: Ice, 2: Fire, 3: Lightning")]
    [SerializeField] private Transform[] spellPositions;
	[Tooltip("1: Ice, 2: Fire, 3: Lightning")]
    [SerializeField] private GameObject[] spellPrefab;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Audio")]
    [Tooltip("0: Hurt, 1: Ice, 2: Fire, 3: Lightning")]
    [SerializeField] private PlayRandomAudio[] audios;

    private float stopTimer = 0;
    private float stopDelay = 6;
    private bool stopMove = false;

    void Start()
    {
        player = Camera.main.transform;
    }

    void Update()
    {
		animator.SetFloat("Speed", agent.velocity.magnitude / agent.speed);

        Vector3 playerLook = player.transform.position;
        playerLook.y = transform.position.y;
		transform.LookAt(playerLook);
        //print(getDistanceFromPlayer() > MaxDistanceFromPlayer);

        if (stopMove)
        {
            if (stopTimer > stopDelay)
            {
                stopMove = false;
                agent.isStopped = false;
                stopTimer = 0;
                stopDelay = Random.Range(3, 9);
            }
            else
            {
                stopTimer += Time.deltaTime;
                animator.SetFloat("Speed", 0);
				return;
            }
        }


        if (getDistanceFromPlayer() > MaxDistanceFromPlayer)
        {
            moveToPlayer();
        }
        else
        {
            if (attackTimer > currentAttackDelay)
            {
                startSpellAttack();
                attackTimer = 0;
            }
            else
            {
                attackTimer += Time.deltaTime;
                checkDistanceToTarget();
            }

        }



        
    }



#region movement
    private void checkDistanceToTarget()
    {
        Vector3 temp = player.transform.position;
        temp.y = transform.position.y;

        if (Vector3.Distance(temp, walkTarget) > maxCircleDistance) { getPositionAroundPlayer(); }

        if (walkTarget == null || walkTarget == Vector3.zero)
        {
            getPositionAroundPlayer();
            return;
        }

        if (Vector3.Distance(transform.position, walkTarget) < minStopDistance)
        {
            getPositionAroundPlayer();
            return;
        }

        agent.destination = walkTarget;
    }

	private float getDistanceFromPlayer()
    {
        //print((transform.position - player.position).magnitude);
        return (transform.position - player.position).magnitude;
    }

    private void getPositionAroundPlayer()
    {
        stopMove = true;
		agent.isStopped = true;
		Vector2 dir = Vector2.zero;
        dir.x = Random.Range(minCircleDistance, maxCircleDistance);
        dir.x *= (Random.value > .5f) ? -1 : 1;
        dir.y = Random.Range(minCircleDistance, maxCircleDistance);
		dir.y *= (Random.value > .5f) ? -1 : 1;
        //print(dir);

		Vector3 pos = Vector3.zero;

        pos.x = player.transform.position.x + dir.x;
        pos.y = transform.position.y;
        pos.z = player.transform.position.z + dir.y;

		agent.SetDestination(pos);
        walkTarget = pos;
        //print(walkTarget);
	}

    private void moveToPlayer(bool stopOnceClose = false)
    {
        agent.destination = player.position;
    }

    private void changeMoveState()
    {
        agent.isStopped = !agent.isStopped;  
    }
	#endregion

#region Spells

    private void getAttackDelay()
    {
        currentAttackDelay = Random.Range(minSpellAttackDelay, maxSpellAttackDelay);
        attackTimer = 0;
    }

    private void startSpellAttack()
    {
        //int spellId = Random.Range(0, spellPositions.Length);
        int spellId = 2;

        switch (spellId)
        {
            case 0: animator.SetTrigger("Ice"); break;
            case 1: animator.SetTrigger("Fire"); break;
            case 2: animator.SetTrigger("Lightning"); break;
        }
    }

    public void CastSpell(int spellId)
    {
        GameObject spell;
        switch (spellId)
        {
            case 0:
                spell = Instantiate(spellPrefab[0]);
                spell.transform.position = spellPositions[0].position;
                spell.transform.rotation = transform.rotation;
                //animator.SetTrigger("Ice");
                break;
            case 1:
				spell = Instantiate(spellPrefab[1]);
				spell.transform.position = spellPositions[1].position;
				spell.transform.rotation = transform.rotation;
				//animator.SetTrigger("Fire");
				break;
            case 2:
				spell = Instantiate(spellPrefab[2]);
				spell.transform.position = spellPositions[2].position;
				spell.transform.rotation = transform.rotation;
				//animator.SetTrigger("Lightning");
				break;
        }

        getAttackDelay();

    }

    public void DamageTaken()
    {
        int damageAni = Random.Range(0, 3);

        animator.SetFloat("DamageAnim", damageAni);
    }

	#endregion

#region Audio
    public void playVoiceAudio(int selection)
    {
        switch (selection)
        {
            case 0: audios[0].playRandAudio(); break;
            case 1: audios[1].playRandAudio(); break;
            case 2: audios[2].playRandAudio(); break;
             case 3: audios[3].playRandAudio(); break;
        }
    }

	#endregion

}
