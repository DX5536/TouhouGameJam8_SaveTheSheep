using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorVisible : MonoBehaviour
{
    // Start is called before the first frame update
    public bool visible;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(visible)
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }

    }
}
