using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using System.Threading;

public class GameManager : MonoBehaviour
{
    [SerializeField] private NavMeshSurface _navMeshSurface;

	public delegate void OnRebakeNaveMesh();
	public static OnRebakeNaveMesh onRebakeNaveMesh;

	// Start is called before the first frame update
	void Start()
    {
        onRebakeNaveMesh += NavMeshUpdate;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void NavMeshUpdate()
    {
		_navMeshSurface.BuildNavMesh();
	}
}
