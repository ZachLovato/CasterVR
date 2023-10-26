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

    bool didEndScript = false;

    [SerializeField] private GameObject lightingSphere;
    [SerializeField] private GameObject lightingWave;

    private bool isFirstCast = true;

    [HideInInspector] public activateSpellcating asc;
    [HideInInspector] public GameObject loc;

    private GameObject dometracker;
    #endregion

    [SerializeField] private bool isDebugging = false;

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
                    printDebug("spell Higher casting");
                    // create a object that will do damage ina circle
                    castDome();
					break;
				case 1:
					printDebug("spell Lower casting");
                    // ground attack or shock "Wave"
                    castWave();
					break;
				case -1:
					printDebug("Error Calculating Distance");
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
            Vector3 spawnLoc = transform.position;
            spawnLoc.y = - 100;
            go.transform.position = spawnLoc;
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

    private void printDebug(string msg)
    {
        if(isDebugging)
        {
            Debug.Log(msg);
        }
    }

}
