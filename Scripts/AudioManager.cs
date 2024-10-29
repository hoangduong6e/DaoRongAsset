using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioSource SoundBg;
    private void Start()
    {
        SoundBg = GetComponent<AudioSource>();
    }
    public static void PlaySound(string namesound)
    {
        if(Setting.hieuungamthanh) LeanAudio.play(Resources.Load("GameData/Sound/" + namesound) as AudioClip);////
    }
    public static void SoundClick()
    {
        if (Setting.hieuungamthanh) LeanAudio.play(Resources.Load("GameData/Sound/soundClick") as AudioClip);
    }
    public static void SetSoundBg(string sound,bool linkkhac = false)
    {
        if (Setting.amthanh)
        {
            if(!linkkhac) SoundBg.clip = Resources.Load("GameData/Sound/" + sound) as AudioClip;
            else SoundBg.clip = Resources.Load(sound) as AudioClip;
            SoundBg.Play();
        }
    }    
    public static void SetSoundBg(AudioClip audio)
    {
        SoundBg.clip = audio;
        SoundBg.Play();
    }
}
