using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Wall : MonoBehaviour
{
	[HideInInspector] public GameObject pointerPosition;
	[HideInInspector] public InputActionProperty handGrip;

	[HideInInspector] public Vector3 startPoint;

    [SerializeField] public float scaleSpeed = 1;
    [SerializeField] public bool debuging = false;

    private bool isScriptDone = false;

    public activateSpellcating asc;

    public Vector3 normal;
    public Quaternion upRotation;

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

        if (handGrip.action.inProgress)
        {
            scaleWall();
            isScriptDone = true;
        }
        else if (isScriptDone)
        {
            this.enabled = false;
            asc.ResetFirstSpell();
        }

    }

    private void scaleWall()
    {
		//float dis = Vector3.Distance(startPoint, pointerPosition.transform.position);
		float dis = pointerPosition.transform.position.y - startPoint.y;
        dis = Mathf.Clamp(dis, -4, 2);

        if (dis < 0)
        {
			transform.rotation = Quaternion.LookRotation(Vector3.Cross(transform.right, normal), normal);
		}
        Vector3 scale = Vector3.one;
        scale.y *= dis * scaleSpeed;

		transform.localScale = scale;
	}

}
