using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class activateSpellcating : MonoBehaviour
{
	// Spell timer 
	private bool startTimer = false;


    [SerializeField] GameObject handPointer;
    [SerializeField] GameObject handPointer2;
	[SerializeField] GameObject hitObj;
    [SerializeField] GameObject SpellStartPoint;

	[Header("Action Input")]
	[SerializeField] private InputActionProperty spellCastTrigger;
	[SerializeField] private InputActionProperty spellCastGrip;
	[SerializeField] private InputActionProperty handPosition;
	[SerializeField] private InputActionProperty handRotation;

    private bool isHeldPrev = false;
    private bool isSpellActive = false;
    private bool isCastSpell = false;
    private bool isFirstCast = true;

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
    }

    // Update is called once per frame
    void Update()
    {
		//enters the casting result
		if (castResult.Match.Name != null)
		{
			if (castResult.Score < lowestSpellScore) return;
			switch (castResult.Match.Name)
			{
				case "Ice":
					castWallSpell();
					break;
				case "Fire":
					castFireSpell();
					break;
			}
		}
		// reads the current grip 
        else
        {
			bool currHeld = spellCastTrigger.action.inProgress;
			isCastSpell = spellCastGrip.action.inProgress;
			Vector3 pos = handPosition.action.ReadValue<Vector3>();

			if (currHeld != isHeldPrev)
			{
				isSpellActive = !isSpellActive;
			}
			if (isSpellActive)
			{
				//castWallSpell();
				SpellBoard();
			}
			else
			{
				CallNClear();
			}



			isHeldPrev = currHeld;
		}

        
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
		}
	}

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

			if (isCastSpell && isFirstCast)
            {
                castWall(hit);
            }
		}
        else
        {
			Debug.DrawLine(ray.origin, ray.origin + ray.direction * 40, Color.green);
		}
		
	}

	private Ray pointerRayCast()
	{
		Vector3 dir = handPointer.transform.localPosition - handPointer2.transform.localPosition;

		Ray ray = new Ray(transform.position, dir);

		return ray;
	}

    private void castWall(RaycastHit hit)
    {
        GameObject wall = Instantiate(IceWall);
        Wall wallCom = wall.GetComponent<Wall>();
		wallCom.startPoint = hit.point;
		wallCom.handGrip = spellCastGrip;
		wallCom.pointerPosition = handPointer;
        isFirstCast = false;
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
