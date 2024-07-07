using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
public class SoundManager : Singleton<SoundManager>
{
    
    [field: SerializeField] public AudioSource audioSourceMusic { get; private set; }
    private Dictionary<AudioClip,AudioSource > audioSourceFXDic = new Dictionary<AudioClip, AudioSource>();
    [SerializeField] private DataSound dataSound;
    public static DataSound DataSound => Instance.dataSound;
    private Tween tweenDecreaseVolumneMusic;
    private void Start()
    {
        Setup();
    }
    public void Setup()
    {
        Mute(SaveGameManager.Instance.IsFXSound);
        MuteMusic(SaveGameManager.Instance.IsSound);
    }
    public SoundManager PlayAudio(AudioClip audioClip, float vollume = 1, bool isLoop = false)
    {
        if (!audioSourceFXDic.ContainsKey(audioClip))
        {
            AudioSource newSource = new GameObject().AddComponent<AudioSource>();
            newSource.transform.SetParent(transform);
            newSource.clip = audioClip;
            newSource.loop = isLoop;
            newSource.mute = !SaveGameManager.Instance.IsFXSound;
            audioSourceFXDic.Add(audioClip, newSource);
        }
        audioSourceFXDic[audioClip].PlayOneShot(audioClip, vollume);
        return this;
    }
    public SoundManager Mute(bool isFXOn)
    {
        foreach (var item  in audioSourceFXDic)
        {
            item.Value.mute = !isFXOn;
        }
        SaveGameManager.Instance.IsFXSound = isFXOn;
        return this;
    }
    public SoundManager MuteMusic(bool isMusicOn)
    {
        if (audioSourceMusic == null)
        {
            audioSourceMusic = gameObject.AddComponent<AudioSource>();
            audioSourceMusic.loop = true;
        }
        audioSourceMusic.mute = !isMusicOn;
        SaveGameManager.Instance.IsSound = isMusicOn;
        return this;
    }

    
    public SoundManager PlayBgMusic(AudioClip bgMusic)
    {
        audioSourceMusic.volume = 1;
        if(bgMusic == audioSourceMusic.clip) return this;
        if (audioSourceMusic == null)
        {
            audioSourceMusic = gameObject.AddComponent<AudioSource>();
            audioSourceMusic.loop = true;
        }
        audioSourceMusic.Stop();
        audioSourceMusic.clip = null;
        if (bgMusic != null)
        {
            audioSourceMusic.clip = bgMusic;
            audioSourceMusic.Play();
            tweenDecreaseVolumneMusic?.Kill();
            float start = 1f;
            tweenDecreaseVolumneMusic = DOVirtual.DelayedCall(30, () =>
            {
                DOTween.To(() => start, value =>
                {
                    start = value;
                    audioSourceMusic.volume = start;
                }, 0.2f, 15);
            });
        }
        return this;
    }

    public bool IsPlayAudioClipEnd(AudioClip audioClip)
    {
        if(audioSourceFXDic.ContainsKey(audioClip)) 
            return !audioSourceFXDic[audioClip].isPlaying;
        return true;
    }

    public void StopAudio(AudioClip audioClip)
    {
        if(audioSourceFXDic.ContainsKey(audioClip))
            audioSourceFXDic[audioClip].Stop();
    }
    public void SetVolumMusic(float value)
    {
        audioSourceMusic.volume = value;
    }
    public void SetVolumFX(float value)
    {
        foreach (var item in audioSourceFXDic)
        {
            item.Value.volume = value;
        }
    }
}
