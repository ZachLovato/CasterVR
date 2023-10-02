using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Wall : MonoBehaviour
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

        startPoint.y = pointerPosition.transform.position.y;

        if (debuging) print(startPoint);
	}

    // Update is called once per frame
    void Update()
    {
        if (!isGripLetGo)
        {
			isGripLetGo = !handGrip.action.inProgress;
            scaleWall();
		}
    }

    private void scaleWall()
    {
		//float dis = Vector3.Distance(startPoint, pointerPosition.transform.position);
		float dis = pointerPosition.transform.position.y - startPoint.y;
        dis = Mathf.Clamp(dis, -4, 2);
        Vector3 scale = Vector3.one;
        scale.y *= dis * scaleSpeed;

		transform.localScale = scale; // have the scale change with the had position
	}

}
