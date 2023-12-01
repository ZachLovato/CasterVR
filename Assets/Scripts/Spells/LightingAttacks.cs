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
	[SerializeField] private bool isDegugging = false;

	[SerializeField] private GameObject auraParticle;

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
					SphereCollider sc = GetComponent<SphereCollider>();
					isFirstPass = false;
					sc.radius = 11;

					auraParticle = Instantiate(auraParticle, transform);
					auraParticle.transform.localPosition = Vector3.zero;
					ParticleSystem ps = auraParticle.GetComponent<ParticleSystem>();
					ps.Play();

				}
				break;
		}
		
	}

    private void shockWave()
    {
        if (isFirstPass)
        {
			printDebug("created the Wave");
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
        else printDebug("nope, no rb");
    }

	private void OnTriggerEnter(Collider other)
	{
		//print(other.gameObject.name);
		switch (state)
		{
			case ATTACKSTATE.Wave:
				if (other.gameObject.TryGetComponent(out Health health) && other.gameObject.tag == "Hostile")
				{
					health.AddHealth(-40);
				}
				else printDebug(other.gameObject.name + " is not a Hostile");
				break;
			case ATTACKSTATE.Aura:
				print(other.gameObject.name);
				if (other.gameObject.tag == "Hostile")
				{
					DoTEffect dot = other.AddComponent<DoTEffect>();
					dot.setStats(10, 5, 5);
					dot.startDoTTimer = true;
					printDebug("Hostile found and killing");
				}
				break;
		}


	}
	private void OnCollisionEnter(Collision other)
	{
		switch (state)
		{
			case ATTACKSTATE.Wave:
				if (other.gameObject.TryGetComponent(out Health health) && other.gameObject.tag == "Hostile")
				{
					health.AddHealth(-10);
				}
				else printDebug(other.gameObject.name + " is not a Hostile");
				break;
			case ATTACKSTATE.Aura:
				if (other.gameObject.tag == "Hostile")
				{
					other.gameObject.AddComponent<DoTEffect>();
				}
				break;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Hostile")
		{
			if (other.gameObject.TryGetComponent(out DoTEffect dot))
            {
                Destroy(dot);
            }
		}
	}

	private void printDebug(string msg)
	{
		if (isDegugging) Debug.Log(msg);
	}

}
