using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateButton : MonoBehaviour
{
	[SerializeField] VRUIInteractions vruiInter;

	private void Start()
	{
		vruiInter.onEnter += onRotateSwitch;
		vruiInter.onExit += onRotateSwitch;
	}

	private void onRotateSwitch()
	{
		if (TryGetComponent(out RotateObject button))
		{
			button.enabled = !button.enabled;
		}
	}
}
