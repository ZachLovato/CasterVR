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

    [SerializeField] DollerHolder dh;
    [SerializeField] LineRenderer lr;

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
					//positions.Add(pos * 0.1f);
					positions.Add(pos);
                    addPosition(pos);
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
        return dh.callDoller(positions);
	}

    public void ChangeTracerPos(Vector3 pos)
    {
        tracer.transform.position = pos;
    }

    public void ClearPositions()
    {
        positions.Clear();
        lastPos = Vector2.zero;
        lr.positionCount = 0;
    }

    public void RecordSpell(string name)
    {
        dh.SavePattern(name, positions);

	}

	private void addPosition(Vector3 position)
	{
        //Vector3 pos = transform.position;
        //pos.x += position.x;
        //pos.y += position.y;
		lr.positionCount++;
		lr.SetPosition(lr.positionCount - 1, position);
	}

}
