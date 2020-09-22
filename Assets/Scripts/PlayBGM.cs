using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBGM : MonoBehaviour
{
    public AudioClip[] bgm;
    private AudioSource audioSource;
    private int nowPlaying;
    void Awake()
    {
        audioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        nowPlaying = Random.Range(0, bgm.Length);
        audioSource.clip = bgm[nowPlaying];
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            nowPlaying = (nowPlaying + 1) % bgm.Length;
            audioSource.clip = bgm[nowPlaying];
            audioSource.Play();
        }
    }
}
