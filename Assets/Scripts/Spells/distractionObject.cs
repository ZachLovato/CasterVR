using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class distractionObject : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.rotation = Quaternion.Euler(0, 90, 0);
    }

	private void FixedUpdate()
	{
        rb.AddForce(transform.forward * 3, ForceMode.Force);
	}

}
