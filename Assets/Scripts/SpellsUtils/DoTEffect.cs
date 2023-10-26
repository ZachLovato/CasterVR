using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoTEffect : MonoBehaviour
{
	public float duration;
	public float damage;
	public bool startDoTTimer;
    public float framePerDamageTick;

    private float timer;
    private float tick;

    Health health;

	private void Start()
	{
		health = GetComponent<Health>();
        Debug.Log("Dot on " + gameObject.name);
	}

	void Update()
    {
        if (startDoTTimer)
        {
            timer += Time.deltaTime;

            if (timer >= duration)
            {
                Destroy(this);
            }
        }
    }

	private void FixedUpdate()
	{
        tick++;

        if (tick >= framePerDamageTick)
        {
            health.AddHealth(-damage);
            print("Damage done");
            tick = 0;
        }
	}

    public void setStats(float durationSec, float ticksPerDamage, float damage)
    {
        duration = durationSec;
        framePerDamageTick = ticksPerDamage;
        this.damage = damage;
    }

}
