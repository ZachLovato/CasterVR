using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMenu : MonoBehaviour
{
    [SerializeField] private GameObject handMenuAni;
    [SerializeField] private GameObject handMenuUI;

    private Animator handMenuAnimator;


    [SerializeField] GameObject audioMenu;
    [SerializeField] GameObject spellMenu;

    void Start()
    {
        onSpellPageSelect();
        handMenuAnimator = handMenuAni.GetComponent<Animator>();
    }

    public void ActivateMenu()
    {
        handMenuAni.SetActive(true);
        handMenuUI.SetActive(false);
        handMenuAnimator.SetTrigger("onRestart");
    }

    public void onAudioPageSelect()
    {
        audioMenu.SetActive(true);
        spellMenu.SetActive(false);
    }

    public void onSpellPageSelect()
    {
		audioMenu.SetActive(false);
		spellMenu.SetActive(true);
	}

    public void showMenu()
    {
        handMenuAni.SetActive(false);
		handMenuUI.SetActive(true);
	}

    public void HideMenu()
    {
		handMenuAni.SetActive(false);
		handMenuUI.SetActive(false);
	}

}
