using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Tooltip("0: Calm Music\n1: Battle Music")]
    [SerializeField] AudioSource[] musics;


    void Start()
    {
		musics[0].mute = false;
	}

    public void changeBgMusic()
    {
        musics[0].mute = !musics[0].mute;
        musics[1].mute = !musics[1].mute;
    }
}
