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
            tick = 0;
        }
	}
}
