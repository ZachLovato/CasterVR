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

    private DollarRecognizer dr;

    private int spellIndex = 0;

    [Space, Header("Debugging")]
    [SerializeField] private bool isDebugging = false;

	private void Awake()
	{
		dr = dh.GetRecognizer();
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
                if (isDebugging) print(stroke.Name);
			}
		}
        else
        {
			dr = dh.GetRecognizer();
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
