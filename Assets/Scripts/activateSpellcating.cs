using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class activateSpellcating : MonoBehaviour
{
    [SerializeField] GameObject handPointer;
    [SerializeField] GameObject handPointer2;
	[SerializeField] GameObject hitObj;

	[Header("Action Input")]
	[SerializeField] private InputActionProperty spellCastTrigger;
	[SerializeField] private InputActionProperty spellCastGrip;
	[SerializeField] private InputActionProperty handPosition;
	[SerializeField] private InputActionProperty handRotation;

    private bool isHeldPrev = false;
    private bool isSpellActive = false;
    private bool isCastSpell = false;

    // this is for the case of debuging
    private float spellcastdelay = 10;
    private float cast = 0;

	[Header("Spell Cast Board")]
	[SerializeField] GameObject spellBoard;
	[SerializeField] LayerMask castLayer;
	private SpellCastUI spellCastUI;

    [Header("Spell Prefabs")]
    [SerializeField] GameObject IceWall;
    [SerializeField] LayerMask wallLayer;


	// Start is called before the first frame update
	void Start()
    {
		spellCastUI = spellBoard.GetComponent<SpellCastUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cast <= 0)
        {
			bool currHeld = spellCastTrigger.action.inProgress;
			isCastSpell = spellCastGrip.action.inProgress;
			Vector3 pos = handPosition.action.ReadValue<Vector3>();

			if (currHeld != isHeldPrev)
			{
				isSpellActive = !isSpellActive;
			}


			isHeldPrev = currHeld;

			if (isSpellActive)
			{
				//if (!spellBoard.activeSelf) spellBoard.SetActive(true);
				//castWallSpell();
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

		}
		else
		{
			cast -= Time.deltaTime;
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
			//Debug.Log("Hit: " + hit.collider.name);

			if (isCastSpell)
            {
				cast = spellcastdelay;
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
        
    }

	private void castBallFire()
	{

	}

}
