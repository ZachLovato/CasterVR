using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils.Datums;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fireball : MonoBehaviour
{
	enum FireballState
	{
		SPAWNING,
		HOLDING,
		FLYING
	};

	private FireballState state;


    private Rigidbody rb;

	[Header("Gravity")]
	[SerializeField] private float gravityTimer = 0;
    private bool gravOn = false;

	[HideInInspector] public GameObject holdPos;

	[Header("Hit Layers")]
	[SerializeField] LayerMask GroundLayer;
	[SerializeField] LayerMask HostileLayer;

	[Header("AoE Information")]
	[SerializeField] GameObject AoEOBJ;
	[SerializeField] float radius = 0;
	[SerializeField] private float AoEDamage = 5;
	[SerializeField] float DoTFrameDelay = 30;
	[SerializeField] float DoTDurationSeconds = 10;

	// Direct Damage
	[SerializeField] private float DDamage = 30;

	[Header("Destruction Timer")]
	[SerializeField] float destroyDelay = 0;
	[SerializeField] bool isFlying = false;
	DestoryTimer dt;

	public InputActionProperty spellCastGrip;

	[SerializeField] private float forceScale = 1;
	public activateSpellcating asc;

	private Vector3 prevPos;
	bool doOnce = true;

	// TODO: have the object change states where it stays near the hand and where it flyies basied on velocity



	// Start is called before the first frame update
	void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

		switch (state)
		{
			case FireballState.SPAWNING:
				transform.position = holdPos.transform.position;

				if (spellCastGrip.action.inProgress) state = FireballState.HOLDING;
				break;
			case FireballState.HOLDING:
				transform.position = holdPos.transform.position;
				if (!spellCastGrip.action.inProgress) state = FireballState.FLYING;
				break;
			case FireballState.FLYING:
				
				isGravOn();
				if (doOnce)
				{
					rb.AddForce(Vector3.Normalize(transform.position - prevPos), ForceMode.Impulse);
					asc.ResetFirstSpell();
					doOnce = false;
					DestoryTimer dt = gameObject.AddComponent<DestoryTimer>();
					dt.useTimer = true;
					dt.DestoryDelay = destroyDelay;
					gameObject.GetComponent<DestoryTimer>().useTimer = true;
				}
				
				break;
		}
		//print("Fireball state: " + state);

		prevPos = transform.position;
	}

    private void isGravOn()
    {
		if (!gravOn)
		{
			gravityTimer -= Time.deltaTime;

			if (gravityTimer < 0)
			{
				rb.useGravity = true;
				gravOn = true;
			}
		}
	}
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == 31)
		{
			GameObject go = Instantiate(AoEOBJ, transform);

			go.GetComponent<DoTSource>().setDoTStats(DoTDurationSeconds, DDamage, DoTFrameDelay);

			go.transform.rotation = Quaternion.identity;
			go.transform.localScale = new Vector3(radius, 1, radius);

			transform.DetachChildren();
			
			Destroy(this.gameObject);
		}
		else if (collision.gameObject.layer == 10)
		{
			collision.gameObject.GetComponent<Health>().AddHealth(-DDamage);
			dt.useTimer = false;
			Destroy(this.gameObject);
		}
	}

}
