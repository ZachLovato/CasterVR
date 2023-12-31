using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryTimer : MonoBehaviour
{
    [SerializeField] public float DestoryDelay = 0;
    private float delay = 0;
    [SerializeField] public bool useTimer = true;
    [SerializeField] public bool DestoryParent = false;

    // Start is called before the first frame update
    void Start()
    {
        delay = DestoryDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (useTimer) delay -= Time.deltaTime;

        if (delay < 0)
        {
            if (DestoryParent)
            {
				Destroy(gameObject.transform.parent.gameObject);
			}
            else
            {
				Destroy(gameObject);
			}
            
        }


    }
}
