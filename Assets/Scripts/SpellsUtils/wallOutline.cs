using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallOutline : MonoBehaviour
{
	[SerializeField]
	private LineRenderer lr;
	[SerializeField]
	private List<Vector3> points;
	[SerializeField]
	private GameObject[] pointObjs;
	private float heightOffset = 0.03f;

	[HideInInspector] public Vector3 wallNormal;

	// Start is called before the first frame update
	void Start()
	{
		lr = GetComponent<LineRenderer>();
		if (lr == null) print("Could not find LineRenderer");
		lr.positionCount = 4;
		lr.loop = true;
	}

	private void Awake()
	{
		lr = GetComponent<LineRenderer>();
		if (lr == null) print("Could not find LineRenderer");
		lr.positionCount = 4;
		lr.loop = true;
		setPostitions();
		lr.SetPositions(points.ToArray());
	}

	// Update is called once per frame
	void Update()
	{
		setPostitions();
		lr.SetPositions(points.ToArray());

	}

	private void setPostitions()
	{
		points.Clear();
		points.Add(pointObjs[0].transform.position);
		points.Add(pointObjs[1].transform.position);
		points.Add(pointObjs[2].transform.position);
		points.Add(pointObjs[3].transform.position);

		
	}
}
