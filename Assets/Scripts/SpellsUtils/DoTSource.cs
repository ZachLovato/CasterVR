using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoTSource : MonoBehaviour
{
    [SerializeField] float duration;
	[SerializeField] float damage;
	[SerializeField] float damageTickDelay;

    List<GameObject> gos;
    List<DoTEffect> dots;

	private void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.tag == "Hostile")
        {
            DoTEffect dot = other.AddComponent<DoTEffect>();

            dot.damage = damage;
            dot.duration = duration;
            dot.framePerDamageTick = damageTickDelay;

            gos.Add(other.gameObject);
            dots.Add(dot);
        }
		
	}

	private void OnTriggerExit(Collider other)
	{
        int removeLoc = -1;

        for (int i = 0; i < gos.Count; i++)
        {
            if (gos[i] == other.gameObject)
            {
                removeLoc = i;
				dots[i].startDoTTimer = true;
                break;
            }
        }

        if (removeLoc != -1)
        {
            gos.RemoveAt(removeLoc);
            dots.RemoveAt(removeLoc);
        }
	}

    public void setDoTStats(float durationSeconds, float directDamage, float delayFrame)
    {
        duration = durationSeconds;
        damage = directDamage;
        damageTickDelay = delayFrame;
    }
}
