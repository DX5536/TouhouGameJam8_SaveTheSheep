using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AvatarCursor : MonoBehaviour
{
    public Texture2D[] texture;

    public float idleTime = 5f;
    public UnityEvent OnIdleInputEvent;

    Vector3 m_oldInputPosition = Vector3.zero;
    float m_internalTimer = 0f;
    bool m_sentFlag = false;

    public enum CursorType
    {
        Neutral,
        Left,
        Right
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(texture[0], new Vector2(0, 0), CursorMode.ForceSoftware);

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        if (IsVectorEqual(m_oldInputPosition, mousePosition))
        {
            m_internalTimer += Time.deltaTime;
            m_internalTimer = Mathf.Min(idleTime, m_internalTimer);
        }
        else
        {
            m_oldInputPosition = mousePosition;
            m_internalTimer = 0f;
            m_sentFlag = false;
        }

        if (m_internalTimer == idleTime)
        {
            Cursor.SetCursor(texture[0], new Vector2(0, 0), CursorMode.ForceSoftware);
        }

        if (Input.GetAxis("Mouse X") < 0)
        {
            //Code for action on mouse moving left
            Cursor.SetCursor(texture[1], new Vector2(0, 0), CursorMode.ForceSoftware);

            print("Mouse moved left");
        }
        if (Input.GetAxis("Mouse X") > 0)
        {
            //Code for action on mouse moving right
            Cursor.SetCursor(texture[2], new Vector2(0, 0), CursorMode.ForceSoftware);

            print("Mouse moved right");
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Cursor.SetCursor(texture[0], new Vector2(0, 0), CursorMode.ForceSoftware);

        }
    }

    bool IsVectorEqual(Vector3 vec1, Vector3 vec2)
    {
        return (vec1.x == vec2.x && vec1.y == vec2.y && vec1.z == vec2.z);
    }
}
