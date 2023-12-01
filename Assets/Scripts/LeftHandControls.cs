using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LeftHandControls : MonoBehaviour
{
	[SerializeField] public InputActionProperty menuButton;

	[SerializeField] private GameObject handMenu;
	private HandMenu hm;


	private float buttonDelay = 2f;
	private float currTime = 0;

	void Start()
	{
		hm = handMenu.GetComponent<HandMenu>();
	}

	private void FixedUpdate()
	{
		if (menuButton.action.inProgress)
		{
			if (currTime >= buttonDelay)
			{
				if (handMenu.activeSelf)
				{
					hm.HideMenu();
					handMenu.SetActive(false);
				}
				else
				{
					hm.ActivateMenu();

					handMenu.SetActive(true);
				}
				currTime = 0;
			}
		}

		if (currTime <= buttonDelay)
		{
			currTime += Time.deltaTime;
		}
	}
}
