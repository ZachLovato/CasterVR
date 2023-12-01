using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEngine.Rendering.DebugUI;

public class AudioSliderChange : MonoBehaviour
{
    [SerializeField] private string audioSettingName;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI num_TMP;
    [SerializeField] private AudioManager audioManager;


    public void changeAudioSetting()
    {
        audioManager.changeAudioValue(audioSettingName, slider.value);

        float value = (slider.value == 0.00001f) ? 0 : slider.value;
        num_TMP.text = (int)(value * 100) + "";
    }

	private void OnEnable()
	{
		if (PlayerPrefs.HasKey(audioSettingName))
        {
            float value = PlayerPrefs.GetFloat(audioSettingName);
            slider.value = value;
			num_TMP.text = (int)(value * 100) + "";
			audioManager.changeAudioValue(audioSettingName, slider.value);
		}
	}
	private void OnDisable()
	{
        PlayerPrefs.SetFloat("audioSettingName", slider.value);
	}

}
