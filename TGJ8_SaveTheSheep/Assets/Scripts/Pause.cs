using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public delegate void PauseState();
    public static event PauseState pause;

    public delegate void ResumeState();
    public static event ResumeState resume;

    public Image fadeScreen;
    public GameObject buttonSet;
    public GameObject blurb;

    public void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            PauseGame();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        Debug.Log("Pause");
        pause();
        StartCoroutine(FadeRoutine(1f, 5f, true));
        buttonSet.SetActive(true);
        blurb.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        Debug.Log("Resume");
        resume();
        buttonSet.SetActive(false);
        blurb.SetActive(false);
        StartCoroutine(FadeRoutine(0f, 5f, false));
    }

    IEnumerator FadeRoutine(float targetAlpha, float fadeSpeed, bool fadeToBlack)
    {
        Color objColor = fadeScreen.color;
        float fadeAmount;

        if (fadeToBlack)
        {
            while (fadeScreen.color.a < targetAlpha)
            {
                fadeAmount = objColor.a + (fadeSpeed * Time.unscaledDeltaTime);

                objColor = new Color(objColor.r, objColor.g, objColor.b, fadeAmount);
                fadeScreen.color = objColor;
                yield return null;
            }
        }
        else
        {
            while (fadeScreen.color.a > targetAlpha)
            {
                fadeAmount = objColor.a - (fadeSpeed * Time.unscaledDeltaTime);

                objColor = new Color(objColor.r, objColor.g, objColor.b, fadeAmount);
                fadeScreen.color = objColor;
                yield return null;
            }
        }

    }
}
