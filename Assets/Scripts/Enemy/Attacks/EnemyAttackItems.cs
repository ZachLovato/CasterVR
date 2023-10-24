using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackItems : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    public enum ATK_TYPE
    {
        Fireball,
        IceWall,
        ShockWave,
        Wind
    }

    public ATK_TYPE type;

    private bool isFirstTime = true;

    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (type)
        {
            case ATK_TYPE.Fireball:
                rb.AddForce(transform.forward * 5, ForceMode.Force);
                break;
            case ATK_TYPE.IceWall:
                if (isFirstTime)
                {
                    GetComponent<DestoryTimer>().useTimer = true;
                    isFirstTime = false;
                }
                break;
            case ATK_TYPE.ShockWave:
				rb.AddForce(transform.forward * 5, ForceMode.Force);
				break;
            case ATK_TYPE.Wind:
                break;

        }
    }


	private void OnTriggerEnter(Collider other)
	{
		if (type != ATK_TYPE.IceWall)
        {
            if (other.gameObject.tag == "Player")
            {
                GetComponent<Health>().AddHealth(-10);
            }
        }
        Destroy(gameObject);
	}
}
