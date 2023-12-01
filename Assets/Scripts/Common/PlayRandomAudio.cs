using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayRandomAudio : MonoBehaviour
{

    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioSource source;

    [SerializeField] public bool useTimer = false;

    [SerializeField] private bool startTimer = false;
    [SerializeField] private float timer;

    private float currTime = 0;

    void Update()
    {
        if (!useTimer) return;

        if (!startTimer) return;

        if (timer >= currTime)
        {
            playRandAudio();
            currTime = 0;
        }
        else currTime += Time.deltaTime;
    }

    public void playRandAudio()
    {
        source.mute = false;

        int randNum = Random.Range(0, clips.Length);

        //if (randNum < clips.Length) return;

        source.clip = clips[randNum];

        source.Play();

    }

    public void muteAudio()
    {
        source.mute = true;
    }
}
