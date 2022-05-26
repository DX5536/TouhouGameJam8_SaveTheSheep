using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleActiveGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        GameManager.enter += SetActive;
        GameManager.play += SetInactive;
    }

    private void OnDisable()
    {
        GameManager.enter -= SetActive;
        GameManager.play -= SetInactive;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetActive()
    {
        for(int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(true);

        }
    }

    void SetInactive()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
