using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] GameObject[] objects;

    [SerializeField] Vector3 normDir;
    [SerializeField] float distance;
    [SerializeField] float angle;

    [SerializeField] bool isAngleLessThen45 = false;


	// Start is called before the first frame update
	void Start()
    {
		objects[0].transform.localPosition = new Vector3(3, 4, 0);
		objects[1].transform.localPosition = new Vector3(3, 8, 0);

	}

    // Update is called once per frame
    void Update()
    {
		normDir = objects[0].transform.localPosition.normalized;
		distance = Vector3.Distance(objects[0].transform.localPosition, objects[1].transform.localPosition);

		objects[2].transform.localPosition = normDir * distance + objects[0].transform.localPosition;

		float pointDist = Vector3.Distance(objects[2].transform.localPosition, objects[1].transform.localPosition);

		float ratio = pointDist / distance;

		float mycalcInRadians = Mathf.Asin(ratio);
		angle = mycalcInRadians * 180 / Mathf.PI;

		if (angle <= 45)
		{
			isAngleLessThen45 = true;
		}
	}
}
