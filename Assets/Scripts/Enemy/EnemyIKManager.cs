using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemyIKManager : MonoBehaviour
{
	[SerializeField] private Animator animator;
    [SerializeField] private Transform player;
	[SerializeField] public bool canLookAtPlayer = false;


	[SerializeField] private MultiAimConstraint head_MAC;

	private void Start()
	{
		player = Camera.main.transform;

		head_MAC.data.sourceObjects.Add(new WeightedTransform(player, 1));
	
	}
	void Update()
    {
        
    }





	private void OnAnimatorIK(int layerIndex)
	{
		lookAtPlayer();
	}

    private void lookAtPlayer()
    {
		if (animator)
		{
			if (canLookAtPlayer)
			{
				if (player != null)
				{
					animator.SetLookAtWeight(1);
					animator.SetLookAtPosition(player.position);
					print("Looking at Player");
				}
			}
			else
			{
				animator.SetLookAtWeight(0);
			}
		}
    }
}
