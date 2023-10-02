using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bridge : MonoBehaviour
{
	private bool isGripLetGo = false;

	[HideInInspector] public GameObject pointerPosition;
	[HideInInspector] public InputActionProperty handGrip;

	[HideInInspector] public Vector3 startPoint;

	[SerializeField] public float scaleSpeed = 1;

	[SerializeField] public bool debuging = false;

	// Start is called before the first frame update
	void Start()
    {
		transform.position = startPoint;

		startPoint.x = pointerPosition.transform.position.x;

		if (debuging) print(startPoint);
	}

    // Update is called once per frame
    void Update()
    {
		if (!isGripLetGo)
		{
			isGripLetGo = !handGrip.action.inProgress;
			scaleBidge();
		}
	}

	private void scaleBidge()
	{
		float dis = pointerPosition.transform.position.x - startPoint.x;
		Vector3 scale = Vector3.one;
		scale.y *= dis * scaleSpeed;

		transform.localScale = scale;
	}
}
