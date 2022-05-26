using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadScene : MonoBehaviour
{
    public string targetScene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            Load();
        }
        
    }

    public void Load()
    {
        LoadingData.sceneToLoad = targetScene;
        SceneManager.LoadScene("LoadingScreen");
    }
}
