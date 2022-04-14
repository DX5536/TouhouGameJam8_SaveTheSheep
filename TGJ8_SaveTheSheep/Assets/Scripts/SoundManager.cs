using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioSource pauseSFX;
    public AudioSource resumeSFX;
    public AudioSource selectSFX;
    public AudioSource playingAudio;
    public AudioSource pauseAudio;
    public AudioMixer TrackA;
    public AudioMixer TrackB;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        Pause.resume += PlayingTrack;
        Pause.pause += PausedTrack;
        Pause.resume += ResumeSFX;
        Pause.pause += PauseSFX;
    }

    private void OnDisable()
    {
        Pause.resume -= PlayingTrack;
        Pause.pause -= PausedTrack;
        Pause.resume -= ResumeSFX;
        Pause.pause -= PauseSFX;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LerpFunction(AudioSource track, float endValue, float duration)
    {
        float time = 0;
        float startValue = track.volume;
        while (time < duration)
        {
            track.volume = Mathf.Lerp(startValue, endValue, time / duration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }
        track.volume = endValue;
    }


    public static IEnumerator StartFade(AudioMixer audioMixer, string exposedParam, float duration, float targetVolume)
    {
        float currentTime = 0;
        float currentVol;
        audioMixer.GetFloat(exposedParam, out currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);
        while (currentTime < duration)
        {
            currentTime += Time.unscaledDeltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            audioMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20);
            yield return null;
        }
        yield break;
    }


    void PlayingTrack()
    {
        //StartCoroutine(LerpFunction(playingAudio, 0.7f, 1f));
        //StartCoroutine(LerpFunction(pauseAudio, 0, 1f));

        StartCoroutine(StartFade(TrackA, "TrackAVolume", 1f, 0.7f));
        StartCoroutine(StartFade(TrackB, "TrackBVolume", 1f, 0.0f));
    }

    void PausedTrack()
    {
        //StartCoroutine(LerpFunction(playingAudio, 0f, 1f));
        //StartCoroutine(LerpFunction(pauseAudio, 0.7f, 1f));

        StartCoroutine(StartFade(TrackA, "TrackAVolume", 1f, 0.0f));
        StartCoroutine(StartFade(TrackB, "TrackBVolume", 1f, 0.7f));
    }

    void ResumeSFX()
    {
        resumeSFX.Play();
    }

    void PauseSFX()
    {
        pauseSFX.Play();
    }
}
