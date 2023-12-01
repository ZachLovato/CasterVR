using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
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
	[SerializeField] float DoTFrameDelay = 30;
	[SerializeField] float DoTDurationSeconds = 10;

	// Direct Damage
	[SerializeField] private float DDamage = 30;

	[Header("Destruction Timer")]
	[SerializeField] float destroyDelay = 0;
	DestoryTimer dt;

	[Header("Particles")]
	[Tooltip("0: idle\n1: trail\n2: pit")]
	[SerializeField] GameObject[] fireParticles;

	[HideInInspector] public InputActionProperty spellCastGrip;
	[HideInInspector] public GameObject userHand;

	//[HideInInspector] public activateSpellcating asc;

	private Vector3 prevPos;
	[SerializeField] private GameObject prevObj;
	bool doOnce = true;

	private bool destorySelf = false;

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		dt = GetComponent<DestoryTimer>();

		fireParticles[0] = Instantiate(fireParticles[0], transform);
		fireParticles[0].transform.position = transform.position;

		fireParticles[1].SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
		if (destorySelf)
		{
			Destroy(gameObject);
			return;
		}
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
					Vector3 temp = transform.position - userHand.transform.position;
					temp = temp.normalized;
					rb.AddForce(temp, ForceMode.Impulse);
					//asc.ResetFirstSpell();
					activateSpellcating.onCastReset();
					doOnce = false;

					// set timer
					DestoryTimer dt = gameObject.AddComponent<DestoryTimer>();
					dt.useTimer = true;
					dt.DestoryDelay = destroyDelay;
					gameObject.GetComponent<DestoryTimer>().useTimer = true;

					// activate Particles
					Destroy(fireParticles[0]);
					Destroy(gameObject.transform.GetChild(0).gameObject);

					fireParticles[1].SetActive(true);
				}
				transform.LookAt(((transform.position - userHand.transform.position).normalized * 2) + transform.position);

				break;
		}

		//print(prevPos + " " + transform.position);
		prevObj.transform.position = prevPos;
		prevPos = userHand.transform.position;
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


	private void spawnAoE(Transform other)
	{
		GameObject go = Instantiate(AoEOBJ, transform);

		go.GetComponent<DoTSource>().setDoTStats(DoTDurationSeconds, DDamage, DoTFrameDelay);

		go.transform.rotation = other.rotation;
		go.transform.localScale = new Vector3(radius, 1, radius);

		go.transform.parent = null;

		//transform.DetachChildren();
	}

	private void spawnAoE(Vector3 pos, Quaternion rot)
	{
		GameObject go = Instantiate(AoEOBJ, transform);

		go.GetComponent<DoTSource>().setDoTStats(DoTDurationSeconds, DDamage, DoTFrameDelay);

		go.transform.rotation = rot;
		go.transform.localScale = new Vector3(radius, 1, radius);
		go.transform.position = pos;

		transform.DetachChildren();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (state != FireballState.FLYING) return;

		fireParticles[3] = Instantiate(fireParticles[3]);
		fireParticles[3].transform.position = transform.position;


		// this checks if the fireball hits the wall layer
		if (collision.gameObject.layer == 31)
		{
			if(collision.gameObject.tag == "Floor")
			{
				spawnAoE(collision.gameObject.transform);
			}
		}
		else if (collision.gameObject.tag == "Hostile")
		{
			collision.gameObject.GetComponent<Health>().AddHealth(-DDamage);
		}

		//Destroy(gameObject.transform.GetChild(0).gameObject);
		//Destroy(gameObject.transform.GetChild(0).gameObject);
		destorySelf = true;
	}

}
