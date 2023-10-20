using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class EnemyVision : MonoBehaviour
{
    private EnemyController _enemyController;

	[SerializeField] private bool isEye = false;

	[Header("Sight Lines")]
	[SerializeField] LayerMask mask;
	[SerializeField, Range(15, 135)] private float angleWidth = 15;
	[SerializeField, Range(5, 21)] private int castAmount = 5;
	[SerializeField, Range(1, 10)] private float sightRange = 1;
	[SerializeField, Range(.1f, 10)] private float radius = 1;

	[SerializeField] private bool isdebugging = false;

    void Start()
    {
        _enemyController = transform.parent.GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
		//if (isEye) transform.rotation = transform.parent.parent.rotation;

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
				_enemyController.FoundPlayer(hit.transform.gameObject);
			}
		}
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
