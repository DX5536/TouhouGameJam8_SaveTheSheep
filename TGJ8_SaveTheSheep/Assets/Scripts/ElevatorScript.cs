using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour, InteractableObject
{
    //TODO
    public bool sheepLock = false; //when enabled, sheep will automatically stop on the elevator and must be dismissed manually
    //TODO
    public bool automatedMovement = false; //when automated, the elevator will move on its own and wont allow manual dragging

    public enum orientation {vertical, horizontal};
    public enum startingPlacement {top, bottom, left, right};
    public startingPlacement startingOrientation = startingPlacement.bottom;
    orientation curOrientation = orientation.vertical;

    public double shaftDepthUnits = 6; //depth of shaft either height or depth wise depending on starting orientation
    public double unitSnap = 0.5; //the elevator will snap to the nearest this unit

    double shaftPeak; //maximum Y or X of the elevator
    double shaftBottom; //minimum Y or X of the elevator

    bool currentlyMouseBound = false;
    float speedMult = 8;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        //register shaft limits
        if (startingOrientation == startingPlacement.bottom)
        {
            shaftBottom = gameObject.transform.position.y;
            shaftPeak = shaftBottom + shaftDepthUnits;
            curOrientation = orientation.vertical;
        }
        else if (startingOrientation == startingPlacement.top)
        {
            shaftPeak = gameObject.transform.position.y;
            shaftBottom = shaftPeak - shaftDepthUnits;
            curOrientation = orientation.vertical;
        }
        else if (startingOrientation == startingPlacement.left)
        {
            shaftBottom = gameObject.transform.position.x;
            shaftPeak = shaftBottom + shaftDepthUnits;
            curOrientation = orientation.horizontal;
        }
        else //if right
        {
            shaftPeak = gameObject.transform.position.x;
            shaftBottom = shaftPeak - shaftDepthUnits;
            curOrientation = orientation.horizontal;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //TODO
        //TODO
        //if sheep lock, if sheep over center of elevator, toggle sheep into a wait mode (sheepbeh or here?)
        //TODO
        //TODO
        //if automated movement, do automated movement cycle
        if(!automatedMovement && currentlyMouseBound)
        {
            if(Input.GetMouseButtonUp(0))
            {
                currentlyMouseBound = false;
                snapElevator();
            } 
            else
            {
                float followDeadzone = 0.07f;
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mouseDirection = new Vector2(mousePos.x - gameObject.transform.position.x, mousePos.y - gameObject.transform.position.y);
                mouseDirection.Normalize();
                float applicableMult = speedMult;
                Debug.Log("Vec to Mouse: "+mouseDirection);
                if((curOrientation == orientation.vertical && ((gameObject.transform.position.y < shaftBottom && mouseDirection.y < 0) || (gameObject.transform.position.y > shaftPeak && mouseDirection.y > 0))) ||
                (curOrientation == orientation.horizontal && ((gameObject.transform.position.x < shaftBottom && mouseDirection.x < 0) || (gameObject.transform.position.x > shaftPeak && mouseDirection.x > 0))) ||
                (Mathf.Abs(mousePos.x - gameObject.transform.position.x) < followDeadzone && Mathf.Abs(mousePos.y - gameObject.transform.position.y) < followDeadzone)) applicableMult = 0;
                gameObject.GetComponent<Rigidbody2D>().velocity = mouseDirection * applicableMult;
            }
        }
    }

    void lockSheep()
    {
        //TODO
    }

    void autoMovementCycle()
    {
        //TODO
    }

    //snaps elevator to nearest unitsnap unit
    void snapElevator()
    {
        double snappedHeight = curOrientation == orientation.vertical ? gameObject.transform.position.y : gameObject.transform.position.x; //grab current location for later snapping
        if(snappedHeight > shaftPeak) snappedHeight = shaftPeak; else if(snappedHeight < shaftBottom) snappedHeight = shaftBottom; //check if out of bounds
        else
        {
            double relativeHeight = ((curOrientation == orientation.vertical ? gameObject.transform.position.y : gameObject.transform.position.x) - shaftBottom); // height on a scale if the bottom of the shaft were 0
            snappedHeight = Mathf.Round((float)(relativeHeight/unitSnap))*unitSnap + shaftBottom; //snapping to the nearest unitsnap unit
            if(snappedHeight > shaftPeak) snappedHeight = shaftPeak; else if (snappedHeight < shaftBottom) snappedHeight = shaftBottom; //check odd rounding errors from depth / snap unit mismatches
        }
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        if(curOrientation == orientation.vertical) gameObject.transform.Translate(new Vector3(0, (float)(snappedHeight - gameObject.transform.position.y), 0));
        else gameObject.transform.Translate(new Vector3((float)(snappedHeight - gameObject.transform.position.x), 0, 0));
    }

    public bool onInteract()
    {
        currentlyMouseBound = true;
        if(curOrientation == orientation.vertical) gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        else gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        //Debug.Log("Elevator interact script successfully procced.");
        return true;
    }
}
