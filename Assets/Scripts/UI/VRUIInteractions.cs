using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRUIInteractions : MonoBehaviour
{
	public delegate void trigEnter();
	public trigEnter onEnter;
	public delegate void trigExit();
	public trigEnter onExit;

	private void OnTriggerEnter(Collider other)
	{
		//print(other.gameObject.name);
		if (onEnter != null) onEnter.Invoke();
	}

	private void OnTriggerExit(Collider other)
	{
		if (onExit != null) onExit.Invoke();
	}

	private void OnCollisionEnter(Collision collision)
	{
		//print(collision.gameObject.name);
	}
}
