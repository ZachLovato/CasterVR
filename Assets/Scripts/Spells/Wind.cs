using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Wind : MonoBehaviour
{
	[SerializeField] bool isDebugging = false;
	[HideInInspector] public InputActionProperty handGrip;

	[HideInInspector] public GameObject handObject;
	[HideInInspector] public Transform spellHoldPos;
	private GameObject initalCastLocation;

	bool didLetGoGrip = false;
	bool firstHeldFrame = true;

	[Header("Wind Attack")]
	[SerializeField] LayerMask mask;
	[SerializeField, Range(15, 135)] private float angleWidth = 15;
	[SerializeField, Range(5, 21)] private int castAmount = 5;
	[SerializeField, Range(1, 10)] private float castRange = 1;
	[SerializeField, Range(1, 10)] private float radius = 1;

	[SerializeField] float damage;
	[SerializeField] GameObject[] WindParticlePrefab;
	[SerializeField] GameObject castObj;

	[SerializeField] LayerMask floor;

	[SerializeField] AudioClip[] clips;
	private AudioSource AudioS;

	private void Start()
	{
		AudioS = GetComponent<AudioSource>();
	}

	void Update()
    {
		bool grip = handGrip.action.inProgress;

		if (grip)
		{

			if (firstHeldFrame)
			{
				initalCastLocation = new GameObject();
				castObj = initalCastLocation;
				initalCastLocation.transform.parent = Camera.main.transform;
				initalCastLocation.transform.position = transform.position;
				firstHeldFrame = false;
			}

			didLetGoGrip = true;
		}
		else if (didLetGoGrip)
		{
			int temp = calculateDistance();

			switch (temp)
			{
				case 0:
					//cast buff
					//debugPrint("Casted a Buff");

					castBuff();
					AudioS.clip = clips[0];
					AudioS.loop = true;
					WindParticlePrefab[1] = Instantiate(WindParticlePrefab[1]);
					WindParticlePrefab[1].transform.parent = Camera.main.transform.parent.transform;
					if (Physics.Raycast(Camera.main.transform.position, -Vector3.up, out RaycastHit hit, 4, floor))
					{
						WindParticlePrefab[1].transform.position = hit.point;
					}

					break;
				case 1:
					//cast attack
					//debugPrint("Casted an Attack");
					attack();
					AudioS.clip = clips[1];
					WindParticlePrefab[0] = Instantiate(WindParticlePrefab[0]);
					WindParticlePrefab[0].transform.rotation = Camera.main.transform.rotation;
					WindParticlePrefab[0].transform.position = transform.position;
					WindParticlePrefab[0].GetComponent<ParticleSystem>().Play();
					AudioS.loop = false;
					break;
			}

			//asc.ResetFirstSpell();
			activateSpellcating.onCastReset();
			Destroy(initalCastLocation);
			Destroy(gameObject);

		}

		//transform.position = handObject.transform.position;
		transform.position = spellHoldPos.position;
    }

	private int calculateDistance()
	{
		float centToHand;
		float centToCast;

		Vector2 center = Vector2.zero;
		center.x = Camera.main.transform.position.x;
		center.y = Camera.main.transform.position.z;

		Vector2 handPosition = Vector2.zero;
		handPosition.x = transform.position.x;
		handPosition.y = transform.position.z;

		Vector2 castPosition = Vector2.zero;
		castPosition.x = initalCastLocation.transform.position.x;
		castPosition.y = initalCastLocation.transform.position.z;

		 centToHand = Vector2.Distance(center, handPosition);
		 centToCast = Vector2.Distance(center, castPosition);

		// if the hand position is closer then cast buff
		// else cast attack
		if (centToHand < centToCast)
		{
			return 0;
		}
        else
        {
            return 1;
        }

    }
	private void castBuff()
	{
		//AdditonalMovement move = 
		//TODO when making a game manager, have the manager give the movement here to buff the move ment
	}
	private void attack()
	{
		List<GameObject> attacked = new List<GameObject>();
		Ray ray;
		Vector3 look = Camera.main.transform.forward;
		float angle = angleWidth / castAmount;
		float currAngle = -(angleWidth * 0.5f);

		for (int i = 0; i <= castAmount; i++)
		{
			ray = new Ray(transform.position, (Quaternion.Euler(0, currAngle, 0) * look).normalized);
			currAngle += angle;
			if (Physics.SphereCast(ray.origin, radius, ray.direction, out RaycastHit hit, castRange, mask))
			{
				// found Hostile
				if (!attacked.Contains(hit.transform.gameObject))
				{
					debugPrint("Hostile Found");
					attacked.Add(hit.transform.gameObject);
					damageHostile(hit.transform.gameObject);
				}
			}
		}
	}
	private void damageHostile(GameObject hostile)
	{
		if (hostile.TryGetComponent(out Health health))
		{
			health.AddHealth(-damage);

			if (health.isObjectDead())
			{
				hostile.GetComponent<EnemyController>().setDeath();
			}
		}

		if (hostile.TryGetComponent<Rigidbody>(out Rigidbody rb))
		{
			Vector3 vel = hostile.transform.position - transform.position;
			vel = vel.normalized;
			rb.AddForce(vel * 300, ForceMode.VelocityChange);
		}

	}



	private void debugPrint(string output)
	{
		if (isDebugging)
		{
			Debug.Log(output);
		}
	}

	private void OnDrawGizmos()
	{
		if (isDebugging)
		{
			Gizmos.color = Color.cyan;
			float angle = angleWidth / castAmount;
			float currAngle = -(angleWidth * 0.5f);
			Ray drawRay;
			Vector3 look = Camera.main.transform.forward;

			for (int i = 0; i <= castAmount; i++)
			{
				drawRay = new Ray(transform.position, Quaternion.Euler(0, currAngle, 0) * look);
				currAngle += angle;
				Gizmos.DrawRay(drawRay.origin, drawRay.direction * castRange);
				Gizmos.DrawWireSphere(drawRay.GetPoint(castRange), radius);
			}
		}
	}

	
}
