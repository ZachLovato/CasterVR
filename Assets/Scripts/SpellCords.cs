using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellCastInfo", menuName = "SpellInfo")]
public class SpellCords : ScriptableObject
{
	public string SpellName;

	public List<Vector2> cords;
}
