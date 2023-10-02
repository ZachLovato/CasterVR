using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class activateSpellcating : MonoBehaviour
{
    [SerializeField] GameObject handPointer;

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


    [Header("Spell Prefabs")]
    [SerializeField] GameObject IceWall;
    [SerializeField] LayerMask wallLayer;


	// Start is called before the first frame update
	void Start()
    {

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
				//castWallSpell();
			}
		}
		else
		{
			cast -= Time.deltaTime;
		}
        
    }

	private void castWallSpell() // renanme this function to a more proper name
	{
        Ray ray = new Ray(handPointer.transform.position, transform.forward);
		float rayLength = 5f;
	    RaycastHit hit;

	    ray.direction = handPointer.transform.rotation * ray.direction;
		if (Physics.Raycast(ray, out hit, rayLength, wallLayer))
		{
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
