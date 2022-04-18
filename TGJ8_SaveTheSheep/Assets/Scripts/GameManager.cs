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

    public GameObject SoundManager;

    public Button button;

    public bool isLast;

    public void Start()
    {
        GetSheepCounts();
        enter();
        Ready();
        button.interactable = true;
        //StartCoroutine(LateStart());
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
        if(current == 0)
        {
            CheckSheep();
        }
    }

    void GetSheepCounts()
    {
        current = sheeps.Length;
        total = sheeps.Length;
        UpdateText();
    }

    void CheckSheep()
    {
        if(delivered == goal)
        {
            StageCleared();
        }
        if(delivered < goal)
        {
            GameOver();
        }
    }

    void UpdateText()
    {
        sheepActiveCount.text = string.Format("{0}/{1}", current.ToString(), total.ToString());
        sheepDeliveredCount.text = string.Format("{0}/{1}", delivered.ToString(), goal.ToString());
    }

    void DetectedSheepDeath()
    {
        current--;
        UpdateText();

    }

    void DetectedSheepDelivered()
    {
        current--;
        delivered++;

        UpdateText();
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
        Debug.Log("GameOver");
        ResetScene();
    }

    void StageCleared()
    {
        Debug.Log("Clear");
        if(isLast)
        {
            Destroy(SoundManager);
        }
        StartCoroutine(WipeSceneRoutine(SceneManager.GetActiveScene().buildIndex+1));
        // load next scene
        //StartCoroutine(WipeSceneRoutine(1));
    }

    void NextScene()
    {
        exit();
        //go into the next scene in the build directory
        //StartCoroutine(WipeSceneRoutine(SceneManager.GetActiveScene().buildIndex));
    }

    public void ResetScene()
    {
        exit();
        StartCoroutine(WipeSceneRoutine(SceneManager.GetActiveScene().buildIndex));
        //SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex));
        //screen wipe
        //load scene current
    }

    public void ReturnToTile()
    {
        exit();
        Destroy(SoundManager);
        StartCoroutine(WipeSceneRoutine(0));
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
