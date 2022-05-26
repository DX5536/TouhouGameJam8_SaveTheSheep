using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManagerV2 : MonoBehaviour
{
    public Dialogue dialogue;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    private Queue<string> sentences;

    public GameObject textBox;
    public GameObject Sprite;

    private string alphanumericText;

    public AudioSource audioSource;
    public AudioClip[] SpeakNoises = new AudioClip[0];
    private bool StillSpeaking;
    private bool specialChar;
    private bool isStopCoroutine;
    // Start is called before the first frame update
    void Awake()
    {
        sentences = new Queue<string>();
    }

    private void Start()
    {
        StartCoroutine(EntryRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DisplayNextSentence();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Skip();
        }
    }

    public IEnumerator EntryRoutine()
    {
        textBox.SetActive(true);
        Sprite.SetActive(true);
        StartCoroutine(FadeRoutine(textBox, 1f, 1f, true));
        StartCoroutine(FadeRoutine(Sprite, 1f, 1f, true));
        yield return new WaitForSecondsRealtime(1f);
        StartDialogue(dialogue);

    }

    public void Exit()
    {
        StartCoroutine(ExitRoutine());

    }
    void Skip()
    {
        EndDialogue();
    }


    public IEnumerator ExitRoutine()
    {
        dialogueText.text = "";
        StartCoroutine(FadeRoutine(textBox, 0f, 1f, false));
        StartCoroutine(FadeRoutine(Sprite, 0f, 1f, false));
        yield return new WaitForSecondsRealtime(1f);
        textBox.SetActive(false);
        Sprite.SetActive(false);
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex+1));


    }

    public void StartDialogue(Dialogue dialogue)
    {
        textBox.SetActive(true);
        Sprite.SetActive(true);
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        nameText.text = dialogue.name;

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(PlayText(sentence, SpeakNoises[0]));
        //dialogueText.text = sentence;
    }

    void EndDialogue()
    {
        isStopCoroutine = true;
        Exit();
        //close dialogue box
    }

    IEnumerator PlayText(string line, AudioClip clip)
    {
        dialogueText.text = "";
        StartCoroutine(SpeakWordsOnLoopRoutine(clip));
        foreach (char c in line)
        {
            if (isStopCoroutine)
                break;
            dialogueText.text += c;
            if (System.Char.IsLetterOrDigit(c))
            {
                alphanumericText += c;
                specialChar = true;

            }
            else
            {
                specialChar = false;

            }
            yield return new WaitForSecondsRealtime(0.03f);
        }
        StillSpeaking = false;
    }

    IEnumerator SpeakWordsOnLoopRoutine(AudioClip clip)
    {
        StillSpeaking = true;
        while (StillSpeaking)
        {
            if (isStopCoroutine)
                break;
            if (specialChar)
            {
                audioSource.Stop();
                audioSource.clip = clip;
                audioSource.Play();
            }
            yield return new WaitForSecondsRealtime(0.03f);

        }
    }

    IEnumerator FadeRoutine(GameObject obj, float targetAlpha, float fadeSpeed, bool fadeToBlack)
    {
        Color objColor = obj.GetComponent<Image>().color;

        float fadeAmount;

        if (fadeToBlack)
        {
            while (objColor.a < targetAlpha)
            {
                fadeAmount = objColor.a + (fadeSpeed * Time.unscaledDeltaTime);

                objColor = new Color(objColor.r, objColor.g, objColor.b, fadeAmount);
                obj.GetComponent<Image>().color = objColor;
                yield return null;
            }
        }
        else
        {
            while (objColor.a > targetAlpha)
            {
                fadeAmount = objColor.a - (fadeSpeed * Time.unscaledDeltaTime);

                objColor = new Color(objColor.r, objColor.g, objColor.b, fadeAmount);
                obj.GetComponent<Image>().color = objColor;
                yield return null;
            }
        }

    }
}
