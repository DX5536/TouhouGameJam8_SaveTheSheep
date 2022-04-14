using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{
    ContactFilter2D sheepFilter;
    // Start is called before the first frame update
    void Start()
    {
        sheepFilter.useLayerMask = true;
        sheepFilter.layerMask = LayerMask.GetMask("Sheep");
    }

    // Update is called once per frame
    void Update()
    {
        //sheep jumps when left click
        if(Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Mouse 0 pressed");
            Collider2D sheepColl = detectSheepAtCurs();
            //Debug.Log("detected collider:" + sheepColl);
            if(sheepColl != null) sheepColl.gameObject.GetComponent<SheepBeh>().mouseJump();
        }
        //turn sheep if right click
        else if (Input.GetMouseButtonDown(1))
        {
            //Debug.Log("Mouse 1 pressed");
            Collider2D sheepColl = detectSheepAtCurs();
            //Debug.Log("detected collider:" + sheepColl);
            if(sheepColl != null) sheepColl.gameObject.GetComponent<SheepBeh>().mouseTurn();
        }
    }

    Collider2D detectSheepAtCurs()
    {
        //Debug.Log("Collider detection called");
        List<RaycastHit2D> hitList = new List<RaycastHit2D>();
        if (Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, sheepFilter, hitList, 0.1f) > 0)
        {
            return hitList[0].collider;
        }
        else return null;
    }

}
