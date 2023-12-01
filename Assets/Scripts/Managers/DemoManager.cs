using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoManager : MonoBehaviour
{
	[SerializeField] private GameObject Ememy;

	[SerializeField] private Transform[] spawnPoints;

	[SerializeField] private int spawnCount;

	[SerializeField] private bool isRandomSpawn;

	private bool doOnce = true;


	private void OnTriggerEnter(Collider other)
	{
		if (doOnce && other.tag == "Player")
		{
			if (isRandomSpawn)
			{
				spawnAtRandomPoints();
			}
			else
			{
				spawnAtPoint();
			}
			doOnce = false;
		}
		
	}

	private void spawnAtPoint()
	{
		for (int i = 0; i < spawnCount; i++)
		{
			GameObject temp = Instantiate(Ememy);
			temp.transform.position = spawnPoints[0].transform.position;
		}
	}

	private void spawnAtRandomPoints()
	{
		for (int i = 0; i < spawnCount; i++)
		{
			int spawnInt = Random.Range(0, spawnPoints.Length);
			GameObject temp = Instantiate(Ememy);
			temp.transform.position= spawnPoints[spawnInt].transform.position;
		}
	}
}
