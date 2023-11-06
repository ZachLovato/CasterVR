using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class EnemyVision : MonoBehaviour
{
	[Header("Sight Lines")]
	[SerializeField] LayerMask mask;
	[SerializeField, Range(15, 135)] private float angleWidth = 15;
	[SerializeField, Range(5, 21)] private int castAmount = 5;
	[SerializeField, Range(1, 10)] private float sightRange = 1;
	[SerializeField, Range(.1f, 10)] private float radius = 1;

	[SerializeField] private bool isdebugging = false;

	private GameObject player;


    void Update()
    {

	}

	public bool isPlayerSeen(bool showDebug = false)
	{
		float angle = angleWidth / castAmount;
		float currAngle = -(angleWidth * 0.5f);
		Ray drawRay;
		Vector3 look = transform.parent.transform.forward;

		for (int i = 0; i <= castAmount; i++)
		{
			drawRay = new Ray(transform.position, Quaternion.Euler(0, currAngle, 0) * look);
			currAngle += angle;

			if (Physics.SphereCast(drawRay, radius, out RaycastHit hit, sightRange, mask))
			{
				if (showDebug) Debug.Log("The player was Seen");
				player = hit.transform.gameObject;
				return true;
				
			}
		}
		player = null;
		return false;
	}

	public GameObject getPlayer()
	{
		return player;
	}

	private void OnDrawGizmos()
	{
		if (isdebugging)
		{
			Gizmos.color = Color.cyan;
			float angle = angleWidth / castAmount;
			float currAngle = -(angleWidth * 0.5f);
			Ray drawRay;
			Vector3 look = transform.parent.transform.forward;

			for (int i = 0; i <= castAmount; i++)
			{
				drawRay = new Ray(transform.position, Quaternion.Euler(0, currAngle, 0) * look);
				currAngle += angle;
				Gizmos.DrawRay(drawRay.origin, drawRay.direction * sightRange);
				Gizmos.DrawWireSphere(drawRay.GetPoint(sightRange), radius);
			}
		}
	}
}
