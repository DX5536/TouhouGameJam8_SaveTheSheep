using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class UITextTypeWriter : MonoBehaviour
{
    public TextMeshProUGUI text;
    public string fullText;
    private string currentText = "";
    private string alphanumericText;
    public AudioSource audioSource;
    public AudioClip[] SpeakNoises = new AudioClip[0];
    private bool StillSpeaking;
    private bool specialChar;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            StartCoroutine(PlayText(SpeakNoises[0]));
        }
    }

    IEnumerator PlayTextRandom()
    {
        StartCoroutine(SpeakRandomWordsOnLoopRoutine(SpeakNoises));
        foreach (char c in fullText)
        {
            text.text += c;
            if(System.Char.IsLetterOrDigit(c))
            {
                alphanumericText += c;
                specialChar = true;

            }
            else
            {
                specialChar = false;

            }
            yield return new WaitForSeconds(0.03f);
        }
        StillSpeaking = false;
    }

    IEnumerator PlayText(AudioClip clip)
    {
        StartCoroutine(SpeakWordsOnLoopRoutine(clip));
        foreach (char c in fullText)
        {
            text.text += c;
            if (System.Char.IsLetterOrDigit(c))
            {
                alphanumericText += c;
                specialChar = true;

            }
            else
            {
                specialChar = false;

            }
            yield return new WaitForSeconds(0.03f);
        }
        StillSpeaking = false;
    }

    IEnumerator SpeakRandomWordsOnLoopRoutine(AudioClip[] clips)
    {
        StillSpeaking = true;
        while(StillSpeaking)
        {
            if(specialChar)
            {
                audioSource.Stop();
                int RandomI = Random.Range(0, clips.Length - 1);
                audioSource.clip = clips[RandomI];
                audioSource.Play();
            }
            yield return new WaitForSeconds(0.03f);

        }
    }

    IEnumerator SpeakWordsOnLoopRoutine(AudioClip clip)
    {
        StillSpeaking = true;
        while (StillSpeaking)
        {
            if (specialChar)
            {
                audioSource.Stop();
                audioSource.clip = clip;
                audioSource.Play();
            }
            yield return new WaitForSeconds(0.03f);

        }
    }
}
