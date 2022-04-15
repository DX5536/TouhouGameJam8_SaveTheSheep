using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeMovementInteractable : MonoBehaviour, InteractableObject
{
    //TODO: find way to recogize offsets in relation to the gameovbject and not the world when it rotates, suspected solution: making use of current object angle and Sin Cos adding the offsets to the input
    //public float xCenterOffset = 0f;
    //public float yCenterOffset = 0f;
    float speedMult = 8f;
    bool isHeldCurrently = false;
    // Update is called once per frame
    void Update()
    {
        if(isHeldCurrently)
        {
            if(!Input.GetMouseButton(0)) isHeldCurrently = false;
            else
            {
                float followDeadzone = 0.08f;
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mouseDirection = new Vector2(mousePos.x - (gameObject.transform.position.x), mousePos.y - (gameObject.transform.position.y));
                mouseDirection.Normalize();
                float applicableMult = speedMult;
                if(Mathf.Abs(mousePos.x - gameObject.transform.position.x) < followDeadzone && Mathf.Abs(mousePos.y - gameObject.transform.position.y) < followDeadzone) applicableMult = 0;
                gameObject.GetComponent<Rigidbody2D>().velocity = mouseDirection * applicableMult;
            }
        }
    }

    public bool onInteract()
    {
        isHeldCurrently = true;
        return true;
    }
}
