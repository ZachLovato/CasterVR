using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DollarRecognizer;

public class SpellCastUI : MonoBehaviour
{
    [HideInInspector] public bool trackPosition = false;

	[SerializeField] float frameDelay = 0;
	float frame = 0;

	[SerializeField] public GameObject tracer;

	[SerializeField] public List<Vector2> positions;

    private Vector2 lastPos = Vector2.zero;

    [SerializeField] bool canAddSpell;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (trackPosition)
        {
			if (frame % frameDelay == 0)
            {
                Vector2 pos = Vector2.zero;
                pos.x = tracer.transform.localPosition.x;
                pos.y = tracer.transform.localPosition.y;

                if (lastPos != pos) 
                {
					positions.Add(pos);
					lastPos = pos;
                }
            }

            // 60 is max frame rate
			frame = (frame >= 60) ? 0 : frame + 1;
		}
        else
        {
            if (frame != 0) frame = 0;
        }
    }

    public DollarRecognizer.Result callDoller()
    {
		DollarRecognizer dr = new DollarRecognizer();

        Result temp = dr.Recognize(positions);

        if (temp.Match.Name == "IceSP") print("IceSP has been casted");
        return temp;
	}

    public void RecordSpell()
    {
        if (positions.Count <= 0) return;

        DollarRecognizer dr = new DollarRecognizer();

		Unistroke us = dr.SavePattern("IceSP", positions);

        Debug.Log(us.Name);
        
    }

    public void ChangeTracerPos(Vector3 pos)
    {
        tracer.transform.position = pos;
    }

    public void ClearPositions()
    {
        positions.Clear();
        lastPos = Vector2.zero;
    }
}
