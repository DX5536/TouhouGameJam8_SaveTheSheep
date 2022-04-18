using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOSceneRef : MonoBehaviour
{
    public SleepSheepScriptableObject so;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnApplicationQuit()
    {
        //so.needDialogue = true;
    }
}
