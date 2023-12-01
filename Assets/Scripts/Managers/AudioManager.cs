using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;

			DontDestroyOnLoad(gameObject);
		}
		else
		{
			DontDestroyOnLoad(gameObject);
		}
	}


	[SerializeField] private AudioMixer _mixer;

    public void changeAudioValue(string name, float value)
    {
        _mixer.SetFloat(name, Mathf.Log10(value) * 20);
    }

}
