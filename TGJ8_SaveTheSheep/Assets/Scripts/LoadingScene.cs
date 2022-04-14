using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadingScene : MonoBehaviour
{
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(LoadingData.sceneToLoad);

        operation.allowSceneActivation = false;

        while(!operation.isDone)
        {
            slider.value = operation.progress;
            //_progressBar.fillAmount = operation.progress;

            if(operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
