using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScriptLogic : MonoBehaviour
{
    public string firstLevelName;
    EmergencyMenuButtons.buttonType thisType;
    public AudioSource clickSound;
    public GameObject creditsMenu;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            clickSound.Play();
            List<RaycastHit2D> hitList = new List<RaycastHit2D>();
            if (Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, _KiroLib.getPanicFilter(), hitList, 0.1f) > 0) //panic implementation, AHAHHHHHHHHHHHHHHHHHHHHHH
            {
                thisType = hitList[0].collider.gameObject.GetComponent<EmergencyMenuButtons>().thisButtonType;
                Debug.Log("object identified: " + thisType);
                hitList[0].collider.gameObject.GetComponent<EmergencyMenuButtons>().triggerClick();
                if(thisType == EmergencyMenuButtons.buttonType.play) loadFirstLevel();
                if(thisType == EmergencyMenuButtons.buttonType.credits) callCredits();
                if(thisType == EmergencyMenuButtons.buttonType.quit) quitGame();
                if(thisType == EmergencyMenuButtons.buttonType.credits_window) closeCredits();
            } else Debug.Log("no target found");
            
        }
    }

    public void loadFirstLevel()
    {
        Debug.Log("level loading");
        SceneManager.LoadScene(firstLevelName);
    }

    public void quitGame()
    {
        Debug.Log("quit called");
        Application.Quit();
    }

    public void callCredits()
    {
        Debug.Log("credits called");
        creditsMenu.SetActive(true);
    }

    public void closeCredits()
    {
        Debug.Log("credits closed");
        creditsMenu.SetActive(false);
    }
}
