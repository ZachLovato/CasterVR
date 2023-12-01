using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static DollarRecognizer;

public class DollerHolder : MonoBehaviour
{
    private DollarRecognizer _recognizer;

	[SerializeField] public SpellCords[] scIniz;

	[Space, Header("Debugging")]
	[SerializeField] private bool isDebuging = false;

	void Start()
    {
        _recognizer = new DollarRecognizer();

		if (_recognizer != null)
		{
			debugPrint("DollarRecognizer created");

			InitializeSpellCords();
		}
		else
		{
			debugPrint("DollarRecognizer Failed to CREATE");
		}
	}

	private void Update()
	{
        
    }

	public DollarRecognizer GetRecognizer() 
	{ 
		return _recognizer; 
	}

	private void InitializeSpellCords()
	{
		string temp = "Initialized: ";

		for (int i = 0; i < scIniz.Count(); i++)
		{
			_recognizer.SavePattern(scIniz[i].SpellName, scIniz[i].cords);
			temp += scIniz[i].SpellName + ", ";
		}

		debugPrint(temp);
	}

	private void debugPrint(string message)
	{
		if (isDebuging) Debug.Log(message);
	}


	#region  DollerR Caller

	public Result callDoller(List<Vector2> positions)
	{
		if (positions.Count < 5) return new Result();
		
		Result r = _recognizer.Recognize(positions);

		debugPrint("1$ has found a Match: " + r.Match.Name + "\nScore: " + r.Score);

		return r;
	}

	public void SavePattern(string spellName, List<Vector2> positions)
	{
		if (positions.Count <= 0) return;

		_recognizer.removeItem(spellName);

		Unistroke us = _recognizer.SavePattern(spellName, positions);

		string allItems = string.Empty;

		List<Unistroke> temp = _recognizer.getUnistrokeList();

		foreach (Unistroke stroke in temp)
		{
			allItems += stroke.Name + ", ";
		}

		debugPrint(allItems);

		ModifySpellHolder(us.Name, positions);
	}

	public void ModifySpellHolder(string name, List<Vector2> pos)
	{
		bool didFindSpellName = false;

		for (int i = 0; i < scIniz.Count();i++)
		{
			if (name == scIniz[i].SpellName)
			{
				scIniz[i].cords.Clear();
				didFindSpellName = true;
				for (int posLoc = 0; posLoc < pos.Count(); posLoc++)
				{
					scIniz[i].cords.Add(pos[posLoc]);
				}
				break;
			}
		}

		if (didFindSpellName) debugPrint("Found " + name + " in list");
		else debugPrint("Could not find " + name + " in spell list");
    }

	#endregion
}
