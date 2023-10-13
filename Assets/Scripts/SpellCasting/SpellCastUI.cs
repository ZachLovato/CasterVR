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

    private DollarRecognizer dr;

	[SerializeField] SpellCords sc;
	[SerializeField] string spellName;

    //[SerializeField] public SpellCords[] scIniz;

    [SerializeField] DollerHolder dh;

	// Start is called before the first frame update
	void Start()
    {
        dr = dh.GetRecognizer();
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
        Result r = dr.Recognize(positions);

        print(r.Match.Name); 
        
        return r;
	}

    public void RecordSpell()
    {
        if (positions.Count <= 0) return;

		Unistroke us = dr.SavePattern(spellName, positions);

        modifyScObj(us.Name, positions);
        
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

    /// <summary>
    /// Modifies the scriptable object so it replaces the name and gives it a new set of cords
    /// </summary>
    /// <param name="name"></param>
    /// <param name="pos"></param>
    private void modifyScObj(string name, List<Vector2> pos)
    {
        sc.SpellName = name;

        sc.cords.Clear();

        for (int i = 0; i < pos.Count; i++)
        {
            sc.cords.Add(pos[i]);
            print(pos[i]);
        }
    }

    public DollarRecognizer getDollarReconizer()
    {
        return dr;
    }
}
