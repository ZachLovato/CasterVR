using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Wind : MonoBehaviour
{
	[SerializeField] bool isDebugging = false;
	[HideInInspector] public InputActionProperty handGrip;

	[HideInInspector] public GameObject handObject;
	[HideInInspector] public activateSpellcating asc;
	private GameObject initalCastLocation;

	bool didLetGoGrip = false;
	bool firstHeldFrame = true;

	[SerializeField] float centToHand;
	[SerializeField] float centToCast;

	[Header("Wind Attack")]
	[SerializeField] LayerMask mask;
	[SerializeField, Range(15, 135)] private float angleWidth = 15;
	[SerializeField, Range(5, 21)] private int castAmount = 5;
	[SerializeField, Range(1, 10)] private float castRange = 1;
	[SerializeField, Range(1, 10)] private float radius = 1;

	[SerializeField] float damage;

	private void Awake()
	{
	}

	// Update is called once per frame
	void Update()
    {
		bool grip = handGrip.action.inProgress;

		if (grip)
		{

			if (firstHeldFrame)
			{
				initalCastLocation = new GameObject();
				initalCastLocation.transform.parent = Camera.main.transform;
				initalCastLocation.transform.position = handObject.transform.position;
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
					debugPrint("Casted a Buff");
					break;
				case 1:
					//cast attack
					debugPrint("Casted an Attack");
					attack();
					break;
			}

			asc.ResetFirstSpell();
			Destroy(initalCastLocation);
			Destroy(gameObject);

		}

		transform.position = handObject.transform.position;
    }

	private int calculateDistance()
	{
		Vector2 center = Vector2.zero;
		center.x = Camera.main.transform.position.x;
		center.y = Camera.main.transform.position.z;

		Vector2 handPosition = Vector2.zero;
		handPosition.x = handObject.transform.position.x;
		handPosition.y = handObject.transform.position.z;

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
