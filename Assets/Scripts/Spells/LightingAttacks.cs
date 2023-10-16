using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightingAttacks : MonoBehaviour
{
    public enum ATTACKSTATE
    {
        Wave,
        Aura
    };

	[SerializeField] private ATTACKSTATE state;
	private bool isFirstPass = true;

	[SerializeField] private Rigidbody rb;
	[SerializeField] private LayerMask lm;

    [SerializeField] private float forceScale;

	private void FixedUpdate()
	{
		switch (state)
		{
			case ATTACKSTATE.Wave:
				shockWave();
                
				break;
			case ATTACKSTATE.Aura:
				if (isFirstPass)
				{
					transform.SetParent(Camera.main.transform);
				}
				break;
		}
	}

    private void shockWave()
    {
        if (isFirstPass)
        {
			print("created the Wave");
			// spawn the wave on the ground
			Ray ray = new Ray(Camera.main.transform.position, Vector3.down);
			Physics.Raycast(ray, out RaycastHit hitInfo, 5, lm);
			
			transform.position = hitInfo.point;

			Vector3 lookDir = Camera.main.transform.rotation.eulerAngles;

			transform.rotation = Quaternion.Euler(0, lookDir.y, 0);

            rb = GetComponent<Rigidbody>();
			isFirstPass = !isFirstPass;
        }
        
        if (rb != null) rb.AddForce(transform.forward * forceScale, ForceMode.Impulse);
        else print("nope, no rb");
    }

	private void OnTriggerEnter(Collider other)
	{
		switch (state)
		{
			case ATTACKSTATE.Wave:
				if (other.gameObject.TryGetComponent(out Health health) && other.gameObject.layer == 10)
				{
					health.AddHealth(10);
				}
				break;
			case ATTACKSTATE.Aura:
				if (other.gameObject.layer == 10)
				{
					other.AddComponent<DoTEffect>();
				}
				break;
		}

		
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == 10)
		{
			if (other.gameObject.TryGetComponent(out DoTEffect dot))
            {
                Destroy(dot);
            }
		}
	}
}
