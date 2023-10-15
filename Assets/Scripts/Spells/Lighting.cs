using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lighting : MonoBehaviour
{
	[HideInInspector] public InputActionProperty handGrip;
	[HideInInspector] public InputActionProperty handPosition;


    private GameObject positionCreated;

    bool didEndScript = false;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool isGripHeld = handGrip.action.ReadValue<bool>();



        if (isGripHeld)
        {
            didEndScript = true;
        }
        else if (didEndScript)
        {
            switch (calculateDistance())
            {
				case 0:
                    print("spell Higher casting");
					break;
				case 1:
					print("spell Lower casting");
					break;
				case -1:
                    Debug.Log("Error Calculating Distance");
					break;
			}
        }

    }

    public int calculateDistance()
    {
        // if the number is -# then its means the hand is above the casted position
        float distance = positionCreated.transform.position.y - handPosition.action.ReadValue<Vector3>().y;

        if (distance < 0) return 0;
        else if (distance > 0) return 1;
        else return -1; // error code
    }


    public void cretePositionTracker(GameObject parent)
    {
		positionCreated = new GameObject();
        positionCreated.transform.SetParent(parent.transform);
	}

}
