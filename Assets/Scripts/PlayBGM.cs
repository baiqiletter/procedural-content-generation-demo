using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBGM : MonoBehaviour
{
    public static PlayBGM Instance;
    public AudioClip[] bgm;
    private AudioSource audioSource;
    private int nowPlaying;
    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        nowPlaying = Random.Range(0, bgm.Length);
        audioSource.clip = bgm[nowPlaying];
        audioSource.Play();
        //print("bgm: awake " + nowPlaying);
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            nowPlaying = (nowPlaying + 1) % bgm.Length;
            audioSource.clip = bgm[nowPlaying];
            audioSource.Play();
            //print("bgm: update " + nowPlaying);
        }
    }
}
