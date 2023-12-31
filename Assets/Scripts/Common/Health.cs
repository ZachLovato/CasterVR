using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float MaxHP;
    [SerializeField] private float HP;

    [SerializeField] private bool isDead = false;
    [SerializeField] private bool isNonPlayer = false;

    [SerializeField, Range(0,20)] private float DestroyDelay = 0;
    private float destroyTimer = 0;

    private EnemyController enemyController;
    private EnemyAttacker enemyAttacker;
    public bool isBotOne = true;

    // Start is called before the first frame update
    void Start()
    {
		destroyTimer = DestroyDelay;
        HP = MaxHP;
        if (isBotOne )
        {
			if (isNonPlayer) enemyController = GetComponent<EnemyController>();
		}
        else if (isNonPlayer) enemyAttacker = GetComponent<EnemyAttacker>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isNonPlayer)
        {
            if (HP <= 0)
            {
                isDead = true;
                WaveManager.onRemoveHostile(gameObject);
            }
        }

        if (isDead)
        {
            if (destroyTimer <= 0)
            {
                gameObject.AddComponent<DestoryTimer>().DestoryDelay = destroyTimer;
            }
            else destroyTimer -= Time.deltaTime;

		}
    }


    // -- public methods -- \\

    public float getHealth() { return HP; }

    public void AddHealth(float hpChange, bool isDoT = false)
    {
        HP += hpChange;

        if (isNonPlayer)
        {
            if (isBotOne)
            {
				if (enemyController == null) enemyController = GetComponent<EnemyController>();
				if (enemyController != null) enemyController.damageTaken(isDoT);
			}
            else
            {
				if (enemyAttacker == null) enemyAttacker = GetComponent<EnemyAttacker>();
				if (enemyAttacker != null) enemyAttacker.DamageTaken();
			}
            
            
		}
    }

    public bool isObjectDead() { return isDead; }



}
