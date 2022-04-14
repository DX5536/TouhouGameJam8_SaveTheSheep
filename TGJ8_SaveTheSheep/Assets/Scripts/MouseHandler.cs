using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //sheep jumps when left click
        if(Input.GetMouseButtonDown(0))
        {
            Collider2D sheepColl = detectSheepAtCurs();
            if(sheepColl != null) sheepColl.gameObject.GetComponent<SheepBeh>().mouseJump();
            else
            {
                Collider2D dBullColl = detectDestructableBulletAtCurs();
                if(dBullColl != null) dBullColl.gameObject.GetComponent<KnifeHandler>().clickDestroy();
            }
        }
        //turn sheep if right click
        else if (Input.GetMouseButtonDown(1))
        {
            Collider2D sheepColl = detectSheepAtCurs();
            if(sheepColl != null) sheepColl.gameObject.GetComponent<SheepBeh>().mouseTurn();
        }
    }

    Collider2D detectSheepAtCurs()
    {
        List<RaycastHit2D> hitList = new List<RaycastHit2D>();
        if (Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, _KiroLib.getSheepFilter(), hitList, 0.1f) > 0)
        {
            return hitList[0].collider;
        }
        else return null;
    }

    Collider2D detectDestructableBulletAtCurs()
    {
        List<RaycastHit2D> hitList = new List<RaycastHit2D>();
        if (Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, _KiroLib.getDBulletFilter(), hitList, 0.1f) > 0)
        {
            return hitList[0].collider;
        }
        else return null;
    }

}