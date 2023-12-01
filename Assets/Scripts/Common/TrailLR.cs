using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailLR : MonoBehaviour
{
    [SerializeField] private LineRenderer lr;

    // Start is called before the first frame update
    void Awake()
    {
        if (lr == null) lr = GetComponent<LineRenderer>();
        lr.positionCount = 0;
    }

    public void addPosition(Vector3 position)
    {
        lr.positionCount++;
        lr.SetPosition(lr.positionCount, position);
    }
}
