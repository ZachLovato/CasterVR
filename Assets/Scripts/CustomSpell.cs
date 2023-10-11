using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static DollarRecognizer;

public class CustomSpell : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI spellName;
    [SerializeField] SpellCastUI spellCastUI;

    private DollarRecognizer dr;

    private int spellIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        dr = spellCastUI.getDollarReconizer();
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
                print(spellIndex);
			}
		}
        else
        {
			dr = spellCastUI.getDollarReconizer();
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

}
