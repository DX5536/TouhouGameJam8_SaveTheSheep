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

    public FollowMouse mouse;

    public bool canPause;

    public void Start()
    {
    }

    private void OnEnable()
    {
        GameManager.play += toggleCanPause;
    }

    private void OnDisable()
    {
        GameManager.play += toggleCanPause;
    }

    // Update is called once per frame
    void Update()
    {
        if(canPause)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        Debug.Log("Pause");
        canPause = false;
        pause();
        mouse.CursorVisible();
        fadeScreen.gameObject.SetActive(true);
        StartCoroutine(FadeRoutine(0.5f, 5f, true));
        buttonSet.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        Debug.Log("Resume");
        canPause = true;
        resume();
        mouse.CursorInvisible();
        buttonSet.SetActive(false);
        StartCoroutine(FadeRoutine(0f, 5f, false));
        fadeScreen.gameObject.SetActive(false);
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

    void toggleCanPause()
    {
        canPause = true;
    }
}
