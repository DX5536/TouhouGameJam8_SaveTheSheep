using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 offset;
    public bool cursorIsVisible;
    void Start()
    {
        Cursor.visible = cursorIsVisible;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = cursorIsVisible;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
        mousePosition.x = mousePosition.x + offset.x;
        mousePosition.y = mousePosition.y + offset.y;
        transform.position = mousePosition;


    }

    public void CursorVisible()
    {
        cursorIsVisible = true;

    }

    public void CursorInvisible()
    {
        cursorIsVisible = false;

    }
}
