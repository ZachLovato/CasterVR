using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public enum ROTATEAXIS
    {
        X, 
        Y, 
        Z
    }

    [SerializeField] private ROTATEAXIS axis;
    [SerializeField] private float speed;

    // Update is called once per frame
    void Update()
    {
        Vector3 rot = transform.localEulerAngles;

		switch (axis)
        {
            case ROTATEAXIS.X:
				rot.x += Time.deltaTime * speed; break;
            case ROTATEAXIS.Y:
				rot.y += Time.deltaTime * speed; break;
			case ROTATEAXIS.Z:
				rot.z += Time.deltaTime * speed; break;
		}

        transform.localEulerAngles = rot;
    }
}
