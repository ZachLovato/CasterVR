using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlayer : MonoBehaviour
{
    [SerializeField] private GameObject[] point;
    [SerializeField] private Transform target;
    [SerializeField] private LayerMask layerMask;

    void Update()
    {
        if (Physics.Raycast(transform.position, point[0].transform.position - point[1].transform.position, out RaycastHit hit, 10, layerMask))
        {
            target.position = hit.point;
        }
        
        
    }

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;




        Gizmos.DrawRay(transform.position, point[0].transform.position - point[1].transform.position * 10);
	}
}
