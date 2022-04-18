using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScriptLogic : MonoBehaviour
{
    public string firstLevelName;

    public void loadFirstLevel()
    {
        SceneManager.LoadScene(firstLevelName);
    }

    public void quitGame()
    {
        Debug.Log("quit called");
        Application.Quit();
    }
}
