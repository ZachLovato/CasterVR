using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyAttacks : MonoBehaviour
{
	enum ATTCKING_STATE
	{
		Circle,
		SpellShot,
		Slap
	}

	[SerializeField] private ATTCKING_STATE state;
	[SerializeField] private GameObject player;
	[SerializeField] private EnemyController enemyController;

	[Space, Header("State Vars")]
	[SerializeField, Range(1,30)] private float CircleDistnce;
	[SerializeField, Range(1,30)] private float maxRadius;
	[SerializeField, Range(1,30)] private float minRadius;
	[SerializeField, Range(.1f,6)] private float meleeRange;

	[Space, SerializeField, Range(1, 60)] private float AttackDelaySec;

	private float circleTimer = 10;
	private float timer;

	[Space, Header("Prefabs")]
	[SerializeField] private GameObject fireSpell;
	[SerializeField] private GameObject iceSpell;
	[SerializeField] private GameObject lightingSpell;
	[SerializeField] GameObject[] attackPosition;

	

	void Start()
    {
		player = Camera.main.transform.parent.transform.parent.gameObject;
    }

	// Update is called once per frame
    void Update()
    {
		switch (state)
		{
			case ATTCKING_STATE.Circle: CirclePlayer(); break;
			case ATTCKING_STATE.SpellShot: SpellShot(); break;
			case ATTCKING_STATE.Slap: Slap(); break;
			default: break;
		}
		/*
		*/
	}

	#region States
	// neutral state, circles around the player
	private void CirclePlayer()
	{
		float dist = Vector3.Distance(transform.position, player.transform.position);
		print(dist);
		if (dist < CircleDistnce)
		{
			if (enemyController != null)
			{
				Vector3 point = GetPointInCircle();
				print(point.x + " " +  point.y + " " + point.z);
				enemyController.setNewTargetPosition(point);
			}
			else print("EnemyController is null");
			
		}

		if (timer >= circleTimer)
		{
			switch (Random.Range(0, 2))
			{
				case 0:
					state = ATTCKING_STATE.SpellShot; break;
				case 1: 
					state = ATTCKING_STATE.Slap; break;
			}
			timer = 0;
		}
		else
		{
			timer += Time.deltaTime;
		}

	}

	// randomly picks one of the spells to use
	private void SpellShot()
	{
		int spell = Random.Range(0, 4 - 1);

		switch (spell)
		{
			case 0:
				// spawns an npc type Fireball
				GameObject go = Instantiate(fireSpell);
				go.GetComponent<EnemyAttackItems>().type = EnemyAttackItems.ATK_TYPE.Fireball;
				go.transform.rotation = transform.rotation;
				break;
			case 1:
				// spawns an npc type WAll
				GameObject gos = Instantiate(iceSpell);
				gos.GetComponent<EnemyAttackItems>().type = EnemyAttackItems.ATK_TYPE.IceWall;
				gos.transform.rotation = transform.rotation;
				break;
			case 2:
				// Lighting wave
				GameObject goss = Instantiate(lightingSpell);
				goss.GetComponent<EnemyAttackItems>().type = EnemyAttackItems.ATK_TYPE.ShockWave;
				goss.transform.rotation = transform.rotation;

				break;
			case 3:
				// Wind
				break;
		}


		state = ATTCKING_STATE.Circle;
	}

	// melee
	private void Slap()
	{
		if (Vector3.Distance(transform.position, player.transform.position) < meleeRange)
		{
			// attack the player
		}
		else
		{
			if (enemyController != null)
			{
				enemyController.setNewTargetPosition(player.transform.position);
			}
			else print("EnemyController is null");
			
		}

		state = ATTCKING_STATE.Circle;
	}
	#endregion

	private Vector3 GetPointInCircle()
	{
		Vector3 dir = player.transform.position - transform.position;
		dir = dir.normalized;
		dir = transform.position + (dir * Random.Range(minRadius, maxRadius));
		return dir;
	}
}
