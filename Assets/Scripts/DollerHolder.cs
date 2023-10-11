using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DollarRecognizer;

public class DollerHolder : MonoBehaviour
{
    private DollarRecognizer _recognizer;

	[SerializeField] SpellCords sc;
	[SerializeField] string spellName;

	[SerializeField] public SpellCords[] scIniz;

	// Start is called before the first frame update
	void Start()
    {
        _recognizer = new DollarRecognizer();

		InitializeSpellCords();

	}

	public DollarRecognizer GetRecognizer() { return _recognizer; }

	private void InitializeSpellCords()
	{
		for (int i = 0; i < scIniz.Count(); i++)
		{
			_recognizer.SavePattern(scIniz[i].SpellName, scIniz[i].cords);
		}
	}
}
