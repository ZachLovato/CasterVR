using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCastUI : MonoBehaviour
{
    [SerializeField] public GameObject tracer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(tracer.transform.localPosition);
    }

    public void ChangeTracerPos(Vector3 pos)
    {
        tracer.transform.position = pos;
    }
}
