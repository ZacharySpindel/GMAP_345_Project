using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sounds[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Awake(){
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    private void Start(){
        // for background music vvvvv
        PlayMusic("DayTheme");
    }

    public void PlayMusic(string name){
        Sounds s = Array.Find(musicSounds, x => x.name == name);

        if (s == null){
            Debug.Log("Sound not found");
        }

        else{
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name){
        Sounds s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null){
            Debug.Log("Sound not found");
        }

        else{
            sfxSource.PlayOneShot(s.clip);
        }
    }

}
