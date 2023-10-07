using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class activateSpellcating : MonoBehaviour
{
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

    private bool isHeldPrev = false;
    private bool isSpellBoardActive = false;
    private bool isFirstCast = true;
    private bool isSpellActive = false;

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
	private bool canRecordSpell = false;

	private GameObject fireballspell = null;


	// Start is called before the first frame update
	void Start()
    {
		spellCastUI = spellBoard.GetComponent<SpellCastUI>();
		spellBoard.SetActive(false);
		castResult = new DollarRecognizer.Result();
    }

    // Update is called once per frame
    void Update()
    {

		bool currHeld = spellCastTrigger.action.inProgress;
		Vector3 pos = handPosition.action.ReadValue<Vector3>();

		if (currHeld != isHeldPrev)
		{
			isSpellBoardActive = !isSpellBoardActive;
		}

		if (isSpellActive)
		{
			switch (castResult.Match.Name)
			{
				case "IceSP":
					castWallSpell();
					break;
				case "FireSP":
					break;
			}
		}
		else if (isSpellBoardActive)
		{
			if (!isFirstCast) isFirstCast = true;
			setSpellBoard();
			SpellBoard();
		}
		else
		{
			if (spellBoard.activeSelf) spellBoard.SetActive(false);

			if (canRecordSpell && currHeld != isHeldPrev)
			{
				Debug.Log("Recorded");
				spellCastUI.RecordSpell();
				spellCastUI.ClearPositions();
			}
			else if (currHeld != isHeldPrev) CallNClear();
		}

		isHeldPrev = currHeld;

		if (handPrimaryButton.action.inProgress)
		{
			canRecordSpell = true;
		}
		else canRecordSpell = false;
			

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

			if (castResult.Score > .6) isSpellActive = true;
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
		isSpellActive = false;
	}

	// -- Spell -- \\

	private void castWallSpell() // renanme this function to a more proper name
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
			fireballspell.GetComponent<Fireball>().holdPos = SpellStartPoint;
		}
	}

}
