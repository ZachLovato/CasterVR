using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class activateSpellcating : MonoBehaviour
{
	enum CastingState
	{
		SPELL,
		CASTING,
		RECORDING,
		RECORDED,
		CLEARING,
		NORMAL
	};

	[Header("State")]
	[SerializeField] CastingState state;

	[Header("GameObjects")]
    [SerializeField] GameObject handPointer;
    [SerializeField] GameObject handPointer2;
    [SerializeField] GameObject handPointer3;
	[SerializeField] GameObject hitObj;
    [SerializeField] GameObject SpellStartPoint;

	[Header("Action Input")]
	[SerializeField] private InputActionProperty spellCastTrigger;
	[SerializeField] private InputActionProperty spellCastGrip;
	[SerializeField] private InputActionProperty handPosition;
	[SerializeField] private InputActionProperty handRotation;
	[SerializeField] private InputActionProperty handPrimaryButton;
	[SerializeField] private InputActionProperty handSecondaryButton;

    private bool isFirstCast = true;
	private bool isRecordingSpell = false;

	[Header("Spell Cast Board")]
	[SerializeField] GameObject spellBoard;
	[SerializeField] LayerMask castLayer;
	private SpellCastUI spellCastUI;

    [Header("Spell Prefabs")]
    [SerializeField] GameObject IceWall;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] GameObject FireBall;

	[SerializeField] float lowestSpellScore = 0.6f;
	private DollarRecognizer.Result castResult;

	private GameObject fireballspell = null;


	// Start is called before the first frame update
	void Start()
    {
		spellCastUI = spellBoard.GetComponent<SpellCastUI>();
		spellBoard.SetActive(false);
		castResult = new DollarRecognizer.Result();
		state = CastingState.NORMAL;
    }

    // Update is called once per frame
    void Update()
    {

		bool currHeld = spellCastTrigger.action.inProgress;
		Vector3 pos = handPosition.action.ReadValue<Vector3>();

		switch (state)
		{
			case CastingState.SPELL:
				switch (castResult.Match.Name)
				{
					case "IceSP":
						wallCastChecker();
						break;
					case "FireSP":
						castFireSpell();
						break;
					default: 
						break;
				}

				if (handSecondaryButton.action.inProgress)
				{
					state = CastingState.NORMAL;
					Destroy( fireballspell );
					fireballspell = null;

				}

				break;
			case CastingState.CASTING:

				if (!isFirstCast) isFirstCast = true;
				
				setSpellBoard();
				SpellBoard();
				
				if (!currHeld) state = CastingState.CLEARING;
				
				break;
			case CastingState.RECORDING:
				
				setSpellBoard();
				SpellBoard();

				if (!currHeld) state = CastingState.RECORDED;

				break;
			case CastingState.RECORDED:
				Debug.Log("Recorded");

				spellCastUI.RecordSpell();
				spellCastUI.ClearPositions();

				state = CastingState.NORMAL;
				
				break;
			case CastingState.CLEARING:
				if (spellBoard.activeSelf) spellBoard.SetActive(false);
				CallNClear();

				// return to normal state

				if (state == CastingState.CLEARING) state = CastingState.NORMAL;
				break;
			case CastingState.NORMAL:
				// sets the spell board to be deativated
				if (spellBoard.activeSelf) spellBoard.SetActive(false);

				Vector3 dir = handPointer.transform.position - handPointer2.transform.position;
				dir = dir.normalized;

				Ray ray = new Ray(transform.position, dir);

				RaycastHit hit;
				if (!Physics.Raycast(ray, out hit, 5, 5))
				{
					if (currHeld && isRecordingSpell) state = CastingState.RECORDING;
					else if (currHeld) state = CastingState.CASTING;
				}

				break;
			default:
				break;
		}

		// ----- OLD SETUP ----- \\
		{
			//if (currHeld != isHeldPrev)
			//{
			//	isSpellBoardActive = !isSpellBoardActive;
			//}

			//if (isSpellActive) // the spell is being called
			//{
			//	switch (castResult.Match.Name)
			//	{
			//		case "IceSP":
			//			castWallSpell();
			//			break;
			//		case "FireSP":
			//			castFireSpell();
			//			break;
			//	}
			//	Debug.Log("Casting " +  castResult.Match.Name + " | Match Results: " + castResult.Score);
			//}
			//else if (isSpellBoardActive) // the board is being called
			//{
			//	if (!isFirstCast) isFirstCast = true;
			//	setSpellBoard();
			//	SpellBoard();
			//}
			//else
			//{
			//	if (spellBoard.activeSelf) spellBoard.SetActive(false);

			//	if (canRecordSpell && currHeld != isHeldPrev)
			//	{
			//		Debug.Log("Recorded");
			//		spellCastUI.RecordSpell();
			//		spellCastUI.ClearPositions();
			//	}
			//	else if (currHeld != isHeldPrev) CallNClear();
			//}

			//isHeldPrev = currHeld;
		}

		if (handPrimaryButton.action.inProgress)
		{
			isRecordingSpell = true;
		}
		else isRecordingSpell = false;

		//print(state);
	}

	// -- Helper -- \\

	private void setSpellBoard()
	{
		if (!spellBoard.activeSelf)
		{
			spellBoard.transform.parent.gameObject.transform.rotation = handPointer3.transform.rotation;
			spellBoard.transform.parent.gameObject.transform.position = handPointer3.transform.position;
		}
		spellBoard.SetActive(true);
	}

	private void SpellBoard()
	{
		if (!spellCastUI.trackPosition) spellCastUI.trackPosition = true;
		Ray ray = pointerRayCast();

		float rayLength = 5f;
		RaycastHit hit;

		ray.direction = handPointer.transform.rotation * ray.direction;
		if (Physics.Raycast(ray, out hit, rayLength, castLayer))
		{
			Debug.DrawLine(ray.origin, hit.point, Color.red);
			spellCastUI.ChangeTracerPos(hit.point);
		}
		else
		{
			Debug.DrawLine(ray.origin, ray.origin + ray.direction * 40, Color.green);
		}
	}

	private void CallNClear()
	{
		if (spellCastUI.trackPosition)
		{
			castResult = spellCastUI.callDoller();
			spellCastUI.trackPosition = false;
			spellCastUI.ClearPositions();

			if (castResult.Score > .6)
			{
				state = CastingState.SPELL;
			}
		}
	}

	private Ray pointerRayCast()
	{
		Vector3 dir = handPointer.transform.localPosition - handPointer2.transform.localPosition;

		Ray ray = new Ray(transform.position, dir);

		return ray;
	}

	public void ResetFirstSpell()
	{
		state = CastingState.NORMAL;
		//isSpellActive = false;

		if (fireballspell != null) fireballspell = null;
	}

	// -- Spell -- \\

	/// <summary>
	/// Checks to see if the position in front of the casting hand is a viable place to put a wall
	/// </summary>
	private void wallCastChecker() // renanme this function to a more proper name
	{
		Vector3 dir = handPointer.transform.localPosition - handPointer2.transform.localPosition;

		Ray ray = new Ray(transform.position, dir);
		float rayLength = 5f;
		RaycastHit hit;

	    ray.direction = handPointer.transform.rotation * ray.direction;

		if (Physics.Raycast(ray, out hit, rayLength, wallLayer))
		{
			hitObj.transform.position = hit.point;
			Debug.DrawLine(ray.origin, hit.point, Color.red);

			if (isFirstCast && spellCastGrip.action.inProgress)
			{
				castWall(hit);
			}

		}
        else
        {
			Debug.DrawLine(ray.origin, ray.origin + ray.direction * 40, Color.green);
		}
		
	}

    private void castWall(RaycastHit hit)
    {
        GameObject wall = Instantiate(IceWall);
        Wall wallCom = wall.GetComponent<Wall>();
		wallCom.startPoint = hit.point;
		wallCom.handGrip = spellCastGrip;
		wallCom.pointerPosition = handPointer;
		wallCom.asc = this;
        isFirstCast = false;

		wall.transform.rotation = Quaternion.LookRotation(Vector3.Cross(wall.transform.right, hit.normal), hit.normal);
		wallCom.upRotation = wall.transform.rotation;

		wallCom.normal = hit.normal;
    }

	private void castFireSpell()
	{
		if (fireballspell == null)
		{
			fireballspell = Instantiate(FireBall);
			Fireball fb = fireballspell.GetComponent<Fireball>();
			fb.holdPos = SpellStartPoint;
			fb.spellCastGrip = spellCastGrip;
			fb.asc = this;
		}
	}

}
