using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public SheepBeh[] sheeps;
    private float current = 0;
    private float total = 0;
    private float delivered = 0;
    public float goal = 0;
    public TMP_Text sheepActiveCount;
    public TMP_Text sheepDeliveredCount;
    public GameObject prepScreen;
    public ScreenWipe sW; 

    public
    // Start is called before the first frame update
    void Start()
    {
        GetSheepCounts();
        sW.ScreenWipeVisible(new Vector3(20f, 20f, 0f), new Vector3(0f, 0f, 0f), 0.5f);
        //UpdateSheepCount();
        Ready();
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
        if(Input.GetKeyDown(KeyCode.K))
        {
            ResetScene();
        }

        if(Input.GetMouseButtonDown(0))
        {
            Play();
        }
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

    void UpdateSheepCount()
    {
        if(current >= 0)
        {
            sheepActiveCount.text = string.Format("{0}/{1}", current.ToString(), total.ToString());
        }

        if (delivered <= goal)
        {
            sheepDeliveredCount.text = string.Format("{0}/{1}", delivered.ToString(), goal.ToString());
        }

        if (current == 0)
        {
            GameOver();
        }

        if (delivered == goal)
        {
            StageCleared();
        }
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
        if (current >= 0)
        {
            current--;
        }
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
        ResetScene();
    }

    void ResetScene()
    {
        StartCoroutine(WipeSceneRoutine(SceneManager.GetActiveScene().buildIndex));
        //SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex));
        //screen wipe
        //load scene current
    }

    IEnumerator WipeSceneRoutine(int index)
    {
        sW.ScreenWipeBlack(new Vector3(0f, 0f, 0f), new Vector3(20f, 20f, 0f), 0.5f);
        float time = 0;

        while (time < 0.5f)
        {
            time += Time.unscaledDeltaTime;
            yield return null;
        }
        SceneManager.LoadScene((index));
    }

    void EnterScene()
    {
        StartCoroutine(EnterRoutine());
        //SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex));
        //screen wipe
        //load scene current
    }

    IEnumerator EnterRoutine()
    {
        //sW.ScreenWipeVisible();
        float time = 0;

        while (time < 0.5f)
        {
            time += Time.unscaledDeltaTime;
            yield return null;
        }

    }

    void StageCleared()
    {
        Debug.Log("Clear");
        ResetScene();
        //Screen wipe
        // load next scene
        //StartCoroutine(WipeSceneRoutine(1));
    }

    void Ready()
    {
        prepScreen.SetActive(true);
        Time.timeScale = 0;
    }

    void Play()
    {
        Time.timeScale = 1;
        prepScreen.SetActive(false);
    }
}
