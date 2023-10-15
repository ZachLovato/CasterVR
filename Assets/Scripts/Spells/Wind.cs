using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{

    [SerializeField] private GameObject pointPreFab;
	private GameObject point;

	private void Awake()
	{
		point = Instantiate(pointPreFab);
	}

	// Update is called once per frame
	void Update()
    {
        
    }


	public void setParent(GameObject newParent)
	{
		point.transform.SetParent(transform, newParent);
	}
}
