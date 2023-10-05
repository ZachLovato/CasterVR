using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils.Datums;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private Rigidbody rb;

	[Header("Gravity")]
	[SerializeField] private float gravityTimer = 0;
    private bool gravOn = false;

	[HideInInspector] public GameObject holdPos;
	private Vector3 position;

	[Header("Hit Layers")]
	[SerializeField] LayerMask GroundLayer;
	[SerializeField] LayerMask HostileLayer;

	[Header("AoE Information")]
	[SerializeField] GameObject AoEOBJ;
	[SerializeField] float radius = 0;
	[SerializeField] private float AoEDamage = 5;
	[SerializeField] private float DDamage = 30;

	[Header("Destruction Timer")]
	[SerializeField] float destroyDelay = 0;
	[SerializeField] bool isFlying = false;
	bool isTimerOn = false;
	DestoryTimer dt;


	// TODO: have the object change states where it stays near the hand and where it flyies basied on velocity



	// Start is called before the first frame update
	void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

		if (isFlying)
		{
			isGravOn();

			if (!isTimerOn)
			{
				dt = gameObject.AddComponent<DestoryTimer>();
				dt.DestoryDelay = destroyDelay;
			}
		}
		else
		{
			transform.position = holdPos.transform.position;
		}

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

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == GroundLayer)
		{
			GameObject go = Instantiate(AoEOBJ, transform);

		}
		else if (other.gameObject.layer == HostileLayer)
		{
			other.GetComponent<Health>().AddHealth(-DDamage);
			dt.useTimer = false;
			Destroy(this.gameObject);
		}		
	}

}
