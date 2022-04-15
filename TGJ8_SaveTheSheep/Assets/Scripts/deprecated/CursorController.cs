using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            anim.SetBool("Pressed", true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            anim.SetBool("Pressed", false);
        }
    }
}
