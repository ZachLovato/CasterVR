using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "SpellInputs", menuName = "SpellInputs")]
public class SpellGrips : ScriptableObject
{
	[SerializeField] public InputActionProperty spellCastTrigger;
	[SerializeField] public InputActionProperty spellCastGrip;
	[SerializeField] public InputActionProperty handPosition;
	[SerializeField] public InputActionProperty handRotation;
	[SerializeField] public InputActionProperty handPrimaryButton;
	[SerializeField] public InputActionProperty handSecondaryButton;
}
