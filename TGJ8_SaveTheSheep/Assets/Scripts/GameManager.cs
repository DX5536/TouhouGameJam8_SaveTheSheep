using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public delegate void EnterState();
    public static event EnterState enter;

    public delegate void dialogueStartState();
    public static event dialogueStartState dialogueStart;

    public delegate void PlayState();
    public static event PlayState play;

    public delegate void ExitState();
    public static event ExitState exit;

    public SheepBeh[] sheeps;
    private float current = 0;
    private float total = 0;
    private float delivered = 0;
    public float goal = 0;
    public TMP_Text sheepActiveCount;
    public TMP_Text sheepDeliveredCount;

    public void Start()
    {
        GetSheepCounts();
        enter();
        Ready();
        StartCoroutine(LateStart());
    }

    public IEnumerator LateStart()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        if(dialogueStart != null)
        {
            dialogueStart();
        }

    }

    private void OnEnable()
    {
        SheepBeh.death += DetectedSheepDeath;
        SheepBeh.delivered += DetectedSheepDelivered;
    }

    private void OnDisable()
    {
        SheepBeh.death -= DetectedSheepDeath;
        SheepBeh.delivered -= DetectedSheepDelivered;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void GetSheepCounts()
    {
        current = sheeps.Length;
        total = sheeps.Length;
        UpdateText();
    }

    void UpdateText()
    {
        sheepActiveCount.text = string.Format("{0}/{1}", current.ToString(), total.ToString());
        sheepDeliveredCount.text = string.Format("{0}/{1}", delivered.ToString(), goal.ToString());
    }

    void DetectedSheepDeath()
    {
        if(current >= 0)
        {
            current--;
        }
        UpdateText();
        ValidateGameState();
    }

    void DetectedSheepDelivered()
    {
        if(delivered < goal)
        {
            delivered++;
        }
        UpdateText();
        ValidateGameState();
    }

    void ValidateGameState()
    {
        if (current == 0)
        {
            GameOver();
        }
        if (delivered == goal)
        {
            StageCleared();
        }
    }

    void GameOver()
    {
        //NextScene();
        ResetScene();
    }

    void StageCleared()
    {
        Debug.Log("Clear");
        ResetScene();
        // load next scene
        //StartCoroutine(WipeSceneRoutine(1));
    }

    void NextScene()
    {
        exit();
        //go into the next scene in the build directory
        //StartCoroutine(WipeSceneRoutine(SceneManager.GetActiveScene().buildIndex));
    }

    void ResetScene()
    {
        exit();
        StartCoroutine(WipeSceneRoutine(SceneManager.GetActiveScene().buildIndex));
        //SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex));
        //screen wipe
        //load scene current
    }

    IEnumerator WipeSceneRoutine(int index)
    {
        float time = 0;

        while (time < 0.5f)
        {
            time += Time.unscaledDeltaTime;
            yield return null;
        }
        SceneManager.LoadScene((index));
    }

    void Ready()
    {
        Time.timeScale = 0;
    }

    public void Play()
    {
        Time.timeScale = 1;
        play();
    }
}
