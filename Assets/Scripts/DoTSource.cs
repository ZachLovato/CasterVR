using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoTSource : MonoBehaviour
{
    float duration;
    float damage;
    bool startDoTTimer;

    List<KeyValuePair<GameObject, DoTEffect>> dotPairs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
		DoTEffect dot = other.AddComponent<DoTEffect>();

        dot.damage = damage;
        dot.duration = duration;

        KeyValuePair<GameObject, DoTEffect> pair = new KeyValuePair<GameObject, DoTEffect>(other.gameObject, dot);


        dotPairs.Add(pair);
	}

	private void OnTriggerExit(Collider other)
	{
        int removeLoc = -1;

        for (int i = 0; i < dotPairs.Count; i++)
        {
            if (dotPairs[i].Key == other.gameObject)
            {
                removeLoc = i;
                dotPairs[i].Value.startDoTTimer = true;
                break;
            }
        }

        if (removeLoc != -1) dotPairs.RemoveAt(removeLoc);
	}
}
