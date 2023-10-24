using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class GameManager : MonoBehaviour
{
    [SerializeField] private NavMeshSurface _navMeshSurface;
    private float navRebuildDelay = 60;
    private float fuTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void FixedUpdate()
	{
        NavMeshUpdate();
	}


    private void NavMeshUpdate()
    {
		if (fuTimer >= navRebuildDelay)
		{
			_navMeshSurface.BuildNavMesh();
			fuTimer = 0;
		}
	}
}
