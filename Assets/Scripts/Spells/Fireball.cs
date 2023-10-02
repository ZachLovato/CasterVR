using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils.Datums;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private Rigidbody rb;

	[SerializeField] private float gravityTimer = 0;
    private bool gravOn = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
		isGravOn();



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



}
