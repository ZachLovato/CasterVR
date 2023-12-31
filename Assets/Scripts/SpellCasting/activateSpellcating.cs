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
    [SerializeField] GameObject SpellStartPoint;
	[SerializeField] GameObject hitObjPrefab;
	[HideInInspector] GameObject hitObj;

	[Header("Action Input")] // go to SpellGrips to add more
	[SerializeField] private SpellGrips interactions;

    private bool isFirstCast = true;
	private bool isRecordingSpell = false;

	[Header("Spell Cast Board")]
	[SerializeField] GameObject spellBoard;
	[SerializeField] LayerMask castLayer;
	[SerializeField, Range(1,5)] float minCastScore = 2.8f;
	private SpellCastUI spellCastUI;

    [Header("Spell Prefabs")]
	[SerializeField] SpellPrefab spellPrefab;
    [SerializeField] LayerMask wallLayer;
	[SerializeField] float rayLength = 10;

	private DollarRecognizer.Result castResult;

	private GameObject spellObject = null;

	private bool prevHeld;

	[SerializeField] CustomSpell customSpell;
	[SerializeField] private animateHandOnInput handAni;
	[SerializeField] private GameObject wand;

	public delegate void ResetCasting();
	public static ResetCasting onCastReset;

	// Start is called before the first frame update
	void Start()
    {
		spellCastUI = spellBoard.GetComponent<SpellCastUI>();
		spellBoard.SetActive(false);
		castResult = new DollarRecognizer.Result();
		state = CastingState.NORMAL;
		hitObj = Instantiate(hitObjPrefab);

		onCastReset += ResetFirstSpell;
    }

    // Update is called once per frame
    void Update()
    {

		bool currHeld = interactions.spellCastTrigger.action.inProgress;


		switch (state)
		{
			case CastingState.SPELL:
				//print(castResult.Score);
				wand.SetActive(true);
				handAni.state = animateHandOnInput.STATES.CASTING;
				switch (castResult.Match.Name)
				{
					case "IceSP":
						wallCastChecker();
						break;
					case "FireSP":
						castFireSpell();
						break;
					case "WindSP":
						castWindSpell();
						break;
					case "LightingSP":
						castLightingSpell();
						break;
					case "SpritSP":
						break;
					default: 
						break;
				}

				if (interactions.handSecondaryButton.action.inProgress)
				{
					state = CastingState.NORMAL;
					Destroy( spellObject );
					spellObject = null;

				}

				break;
			case CastingState.CASTING:
				handAni.state = animateHandOnInput.STATES.CASTING;

				if (!isFirstCast)
				{
					isFirstCast = true;
					hitObj.SetActive(true);
				}
				wand.SetActive(true);
				
				setSpellBoard();
				SpellBoard();
				
				if (!currHeld) state = CastingState.CLEARING;
				
				break;
			case CastingState.RECORDING:
				wand.SetActive(true);
				if (interactions.spellCastTrigger.action.ReadValue<float>() == 1)
				{
					setSpellBoard();
					SpellBoard();
				}

				if (interactions.spellCastTrigger.action.ReadValue<float>() == 0 && prevHeld != currHeld && spellBoard.activeSelf)
				{
					state = CastingState.RECORDED;
					print("Finished Recording");
				}
				
				break;
			case CastingState.RECORDED:
				//Debug.Log("Recorded");
				
				isRecordingSpell = false;

				spellCastUI.RecordSpell(customSpell.getCurrentName());
				spellCastUI.ClearPositions();

				state = CastingState.NORMAL;
				
				break;
			case CastingState.CLEARING:
				if (spellBoard.activeSelf) spellBoard.SetActive(false);
				CallNClear();


				handAni.state = animateHandOnInput.STATES.DEFAULT;
				
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
					handAni.state = animateHandOnInput.STATES.DEFAULT;
				}
				else
				{
					
					if (hit.transform.gameObject.active)
					{
						//print(hit.transform.name);
						handAni.state = animateHandOnInput.STATES.UI;
					}
				}
				wand.SetActive(false);
				
				break;
			default:
				break;
		}

		prevHeld = currHeld;
	}

	#region  Helper

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
			if (castResult.Match == null) return;
			spellCastUI.trackPosition = false;
			spellCastUI.ClearPositions();

			if (castResult.Score > minCastScore)
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

		if (spellObject != null) spellObject = null;
	}

	public void RecordCasting()
	{
		isRecordingSpell = true;
		state = CastingState.RECORDING;
	}

	#endregion

	#region  SpellCasting
	/// <summary>
	/// Checks to see if the position in front of the casting hand is a viable place to put a wall
	/// </summary>
	private void wallCastChecker() // renanme this function to a more proper name
	{
		Vector3 dir = handPointer.transform.localPosition - handPointer2.transform.localPosition;

		Ray ray = new Ray(transform.position, dir);
		RaycastHit hit;

	    ray.direction = handPointer.transform.rotation * ray.direction;

		if (Physics.Raycast(ray, out hit, rayLength, wallLayer))
		{
			if (hit.point != null)
			{
				hitObj.transform.position = hit.point;
				Quaternion temp;
				temp = Quaternion.LookRotation(Vector3.Cross(hitObj.transform.forward, hit.normal), hit.normal);

				hitObj.transform.rotation = temp;
				//print(temp);
				Debug.DrawLine(ray.origin, hit.point, Color.red);

				if (isFirstCast && interactions.spellCastGrip.action.inProgress)
				{
					hitObj.SetActive(false);
					castWall(hit);
				}
			}
		}
        else
        {
			Debug.DrawLine(ray.origin, ray.origin + ray.direction * 40, Color.green);
		}
		
	}

	private void castWall(RaycastHit hit)
    {
        GameObject wall = Instantiate(spellPrefab.IcePrefab);
        Wall wallCom = wall.transform.GetChild(0).GetComponent<Wall>();
		print(wall.transform.GetChild(0));
		wallCom.startPoint = hit.point;
		wallCom.handGrip = interactions.spellCastGrip;
		wallCom.pointerPosition = handPointer;
		//wallCom.asc = this;
        isFirstCast = false;

		wallCom.WandOffset = SpellStartPoint.transform.position;

		//wall.transform.rotation = Quaternion.LookRotation(Vector3.Cross(wall.transform.right, hit.normal), hit.normal);
		wall.transform.rotation = Quaternion.FromToRotation(hit.transform.up, hit.normal);// * transform.rotation;
		wallCom.upRotation = wall.transform.rotation;

		wallCom.normal = hit.normal;
    }

	private void castFireSpell()
	{
		if (spellObject == null)
		{
			spellObject = Instantiate(spellPrefab.FirePrefab);
			Fireball fb = spellObject.GetComponent<Fireball>();
			fb.holdPos = SpellStartPoint;
			fb.spellCastGrip = interactions.spellCastGrip;
			fb.userHand = gameObject;
			//fb.asc = this;
		}
	}

	private void castLightingSpell()
	{
		if (spellObject == null)
		{
			spellObject = Instantiate(spellPrefab.LightingPrefab);
			Lighting light = spellObject.GetComponent<Lighting>();
			//light.asc = this;
			light.loc = SpellStartPoint;
			light.handGrip = interactions.spellCastGrip;
			light.handPosition = gameObject;
		}
	}

	private void castWindSpell()
	{
		if (spellObject == null)
		{
			spellObject = Instantiate(spellPrefab.WindPrefab);
			Wind wind = spellObject.GetComponent<Wind>();
			spellObject.transform.position = SpellStartPoint.transform.position;
			wind.handGrip = interactions.spellCastGrip;
			wind.handObject = gameObject;
			wind.spellHoldPos = SpellStartPoint.transform;
			//wind.asc = this;
		}
	}


	#endregion
}
