using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = new AudioManager();
    public AudioSource backgroundAudioSource;
    public AudioSource SFXAudioSource;
    public List<AudioClip> catSound, backgroundMusic;
    public AudioMixer myMixer;
    public AudioMixerSnapshot main, muted, sfxMuted, bgMuted;

    public bool isMuted;
    public bool isSFXMuted;
    public bool isBGMuted;

    #region Instance and things

    static AudioManager()
    {
    }

    private AudioManager()
    {
    }

    public static AudioManager Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        ShuffleSong();
        backgroundAudioSource.volume = 0;
        backgroundAudioSource.Play();
    }

    void Update()
    {
        if(!isMuted || !isBGMuted)
            GrowAudio();

        if (!backgroundAudioSource.isPlaying)
        {
            ShuffleSong();
        }
    }

    public void PlayRandomCatSound()
    {
        PlayWithRandomVol(catSound[Random.Range(0, catSound.Count)]);
    }

    public void PlayWithRandomVol(AudioClip audioToBePlayed)
    {
        float randomVol = Random.Range(0.1f, 1f);
        SFXAudioSource.PlayOneShot(audioToBePlayed, randomVol);
    }

    public void PlayRandomVolWithSrc(AudioClip audioToBePlayed, AudioSource audioSrcToBeUsed)
    {
        float randomVol = Random.Range(0.1f, 1f);
        audioSrcToBeUsed.PlayOneShot(audioToBePlayed, randomVol);
    }

    public void ShuffleSong()
    {
        int randomSongID = Random.Range(0, backgroundMusic.Count);
        backgroundAudioSource.Stop();
        Debug.Log(randomSongID);
        backgroundAudioSource.PlayOneShot(backgroundMusic[randomSongID]);
    }

    void GrowAudio()
    {
        if (backgroundAudioSource.volume != 1)
        {
            backgroundAudioSource.volume = Mathf.Lerp(backgroundAudioSource.volume, 1, Time.deltaTime * 0.3f);
        }
    }

    public void Mute()
    {
        if (this.isMuted)
        {
            this.isMuted = false;
            main.TransitionTo(0.5f);
        }
        else
        {
            this.isMuted = true;
            muted.TransitionTo(0.5f);
        }
    }

    public void MuteSFX()
    {
        if (this.isSFXMuted)
        {
            this.isSFXMuted = false;
            main.TransitionTo(0.5f);
        }
        else
        {
            this.isSFXMuted = true;
            sfxMuted.TransitionTo(0.5f);
        }
    }

    public void MuteBG()
    {
        if (this.isBGMuted)
        {
            this.isBGMuted = false;
            main.TransitionTo(0.5f);
        }
        else
        {
            this.isBGMuted = true;
            bgMuted.TransitionTo(0.5f);
        }
    }
}
