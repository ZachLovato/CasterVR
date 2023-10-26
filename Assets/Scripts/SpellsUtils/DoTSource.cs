using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoTSource : MonoBehaviour
{
    [SerializeField] float duration;
	[SerializeField] float damage;
	[SerializeField] float damageTickDelay;

	[SerializeField] float Radius;

	SphereCollider sphereCollider;

	bool isGrowth = true;

    List<GameObject> gos;
    List<DoTEffect> dots;

	private void Start()
	{
		sphereCollider = GetComponent<SphereCollider>();
	}

	private void Update()
	{
		if (isGrowth)
		{
			if (sphereCollider.radius < Radius)
			{
				sphereCollider.radius += sphereCollider.radius * 1.2f;
			}
			else
			{
				if (sphereCollider.radius > Radius)
				{
					sphereCollider.radius = Radius;
				}
				isGrowth = false;
			}
		}

		//Collider[] coll = Physics.OverlapSphere(transform.position, Radius);

		//for (int i = 0; i < coll.Length; i++)
		//{
		//	if (coll[i].transform.gameObject.tag == "Hostile")
		//	{

		//	}
		//}

	}


	private void OnTriggerEnter(Collider other)
	{
		print(other.gameObject.name);

        if (other.gameObject.tag == "Hostile")
        {
            DoTEffect dot = other.AddComponent<DoTEffect>();

            dot.damage = damage;
            dot.duration = duration;
            dot.framePerDamageTick = damageTickDelay;
			dot.startDoTTimer = true;

            gos.Add(other.gameObject);
            dots.Add(dot);


        }
		
	}

	private void OnTriggerExit(Collider other)
	{
        if (other.gameObject.tag == "Hostile")
        {
			int removeLoc = -1;

			for (int i = 0; i < gos.Count; i++)
			{
				if (gos[i] == other.gameObject)
				{
					removeLoc = i;
					gos[i].GetComponent<DestoryTimer>().useTimer = true;
					break;
				}
			}

			if (removeLoc != -1)
			{
				gos.RemoveAt(removeLoc);
				dots.RemoveAt(removeLoc);
			}
		}
        
	}

    public void setDoTStats(float durationSeconds, float directDamage, float delayFrame)
    {
        duration = durationSeconds;
        damage = directDamage;
        damageTickDelay = delayFrame;
    }
}
