using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellPrefabs", menuName = "Spellfabs")]
public class SpellPrefab : ScriptableObject
{
    public GameObject IcePrefab;
    public GameObject FirePrefab;
    public GameObject WindPrefab;
    public GameObject LightingPrefab;
    public GameObject SpritPrefab;
}
