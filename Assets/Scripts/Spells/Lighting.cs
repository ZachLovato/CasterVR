using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lighting : MonoBehaviour
{
	#region Lighting Casting Variables
	[HideInInspector] public InputActionProperty handGrip;
	[HideInInspector] public GameObject handPosition;

    private GameObject positionCreated;

    bool didEndScript = false;

    [SerializeField] private GameObject lightingSphere;
    [SerializeField] private GameObject lightingWave;

    private bool isFirstCast = true;

    [HideInInspector] public activateSpellcating asc;
    [HideInInspector] public GameObject loc;

    private GameObject dometracker;
    #endregion

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool isGripHeld = handGrip.action.inProgress;

        transform.position = loc.transform.position;

        if (isGripHeld)
        {
            if (dometracker == null)
            {
                dometracker = new GameObject();
                dometracker.transform.position = transform.position;
            }
            else
            {
                Vector3 tempLoc = transform.position;
                tempLoc.y = dometracker.transform.position.y;
                dometracker.transform.position = tempLoc;
            }
            didEndScript = true;
        }
        else if (didEndScript)
        {
            switch (calculateDistance())
            {
				case 0:
                    print("spell Higher casting");
                    // create a object that will do damage ina circle
                    castDome();
					break;
				case 1:
					print("spell Lower casting");
                    // ground attack or shock "Wave"
                    castWave();
					break;
				case -1:
                    Debug.Log("Error Calculating Distance");
					break;
			}

            Destroy(gameObject);
        }

    }

    private void castDome()
    {
        if (isFirstCast)
        {
            GameObject go = Instantiate(lightingSphere, transform);

            go.transform.SetParent(Camera.main.transform);
            go.transform.localPosition = Vector3.zero;

            asc.ResetFirstSpell();
        }
    }

    private void castWave()
    {
		if (isFirstCast)
		{
			GameObject go = Instantiate(lightingWave);
            go.transform.position = transform.position;
			asc.ResetFirstSpell();
		}
	}

    public int calculateDistance()
    {
        float created = dometracker.transform.position.y;
        float casted = handPosition.transform.position.y;

        // if the number is -# then its means the hand is above the casted position

        Destroy(dometracker);

        if (casted <= created) return 1;
        else if (casted > created) return 0;
        else return -1; // error code
    }

}
