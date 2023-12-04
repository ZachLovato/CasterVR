using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandMenu : MonoBehaviour
{
    [SerializeField] private GameObject handMenuAni;
    [SerializeField] private GameObject handMenuUI;

    [Space]

    private Animator handMenuAnimator;


    [SerializeField] GameObject audioMenu;
    [SerializeField] GameObject spellMenu;
    [SerializeField] GameObject generalMenu;

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
        generalMenu.SetActive(false);
    }

    public void onSpellPageSelect()
    {
		audioMenu.SetActive(false);
		spellMenu.SetActive(true);
		generalMenu.SetActive(false);
	}

    public void onGeneralPageSelect()
    {
		audioMenu.SetActive(false);
		spellMenu.SetActive(false);
		generalMenu.SetActive(true);
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

    public void onQuitGame()
    {
        Application.Quit();
    }

    public void onMainMenu()
    {
		SceneManager.LoadScene("TitleScene", LoadSceneMode.Single);
	}

}
