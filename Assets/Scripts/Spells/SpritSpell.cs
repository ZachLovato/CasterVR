using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpritSpell : MonoBehaviour
{
    [SerializeField] private InputActionProperty handGrip;
    [SerializeField] private LayerMask mask;

    [SerializeField] private GameObject[] spellPrefab;
	[HideInInspector] private GameObject handObject;

	private bool didScriptEnd = false;

    private GameObject spellCastPosition;

	GameObject cam;

	private void Start()
	{
		cam = Camera.main.transform.gameObject;
	}

	private void Update()
	{
		bool isGripHeld = handGrip.action.inProgress;

		if (isGripHeld )
		{
			if (spellCastPosition == null)
			{
				spellCastPosition = new GameObject();
				spellCastPosition.transform.parent = Camera.main.transform;
				spellCastPosition.transform.position = transform.position;
			}

			didScriptEnd = true;
		}
		else if (didScriptEnd) 
		{
			// create a switch statement that would cast disablement or Distraction
			switch (calculateDistance())
			{
				case 0: CastDistraction(); break;
				case 1: CastDisablement(); break;
			}
		}

	}

	private int calculateDistance()
	{
		// returns a value on based on distance from the cast point
		// do based on x z values
		// have the far cast be a beam and 
		// close spawn a distraction

		float centToHand;
		float centToCast;

		Vector2 center = Vector2.zero;
		center.x = Camera.main.transform.position.x;
		center.y = Camera.main.transform.position.z;

		Vector2 handPosition = Vector2.zero;
		handPosition.x = handObject.transform.position.x;
		handPosition.y = handObject.transform.position.z;

		Vector2 castPosition = Vector2.zero;
		castPosition.x = spellCastPosition.transform.position.x;
		castPosition.y = spellCastPosition.transform.position.z;

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

	private void CastDistraction()
	{
		GameObject dis = Instantiate(spellPrefab[0]);
		dis.transform.parent = cam.transform.parent;
		dis.transform.rotation = Quaternion.Euler(0 , cam.transform.forward.y ,0);
	}

	private void CastDisablement()
	{
		if (Physics.SphereCast(transform.position, 3, cam.transform.forward, out RaycastHit hit, 10, mask))
		{
			if (hit.transform.gameObject.tag == "Hostile")
			{
				hit.transform.gameObject.GetComponent<EnemyController>().isDisable = true;
			}
		}
	}

}
