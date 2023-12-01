using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static DollarRecognizer;

public class CustomSpell : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI spellName;
    [SerializeField] DollerHolder dh;
    [SerializeField] activateSpellcating spellCaster;
    [SerializeField] private float scaler = 1;

    private DollarRecognizer dr;

    private int spellIndex = 0;

    [SerializeField] private bool useLineR;
    [SerializeField] private LineRenderer lr;
    [SerializeField] private SpellCords[] scr;

    [Space, Header("Debugging")]
    [SerializeField] private bool isDebugging = false;

	private void Awake()
	{
		dr = dh.GetRecognizer();
        //if (useLineR) lr = GetComponent<LineRenderer>();
	}

	// Update is called once per frame
	void Update()
    {
        //print(spellIndex);
        if (dr != null)
        {
			Unistroke stroke = dr.getStroke(spellIndex);

			if (stroke != null)
			{
				spellName.text = stroke.Name;
                if (useLineR) useLineRend(stroke);
                if (isDebugging) print(stroke.Name);
			}
		}
        else
        {
			dr = dh.GetRecognizer();
		}

        
    }

    private void useLineRend(Unistroke stroke)
    {
        for (int i = 0;  i < scr.Length; i++)
        {
            if (stroke.Name == scr[i].SpellName)
            {
                lr.positionCount = scr[i].cords.Count;
                for (int j = 0; j < scr[i].cords.Count; j++)
                {
                    lr.SetPosition(j, scr[i].cords[j]);
				}
                return;
            }
        }
    }

    public void nextSpell()
    {
        spellIndex++;
        if (spellIndex >= dr.Count())
        {
            spellIndex = 0;
        }
    }

    public void prevSpell()
    {
        spellIndex--;
        if (spellIndex < 0)
        {
			spellIndex = dr.Count() - 1;
        }
    }

    public void TurnOnRecording()
    {
        spellCaster.RecordCasting();
    }

    public string getCurrentName()
    {
        return spellName.text;
    }

}
