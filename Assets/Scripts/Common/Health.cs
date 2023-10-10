using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float MaxHP;
    [SerializeField] private float HP;

    [SerializeField] private bool isDead = false;

    [SerializeField, Range(0,20)] private float DestroyDelay = 0;
    private float destroyTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
		destroyTimer = DestroyDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            if (destroyTimer < 0)
            {
                gameObject.AddComponent<DestoryTimer>().DestoryDelay = destroyTimer;
            }
        }
    }


    // -- public methods -- \\

    public float getHealth() { return HP; }

    public void AddHealth(float hpChange)
    {
        HP += hpChange;
    }



}
