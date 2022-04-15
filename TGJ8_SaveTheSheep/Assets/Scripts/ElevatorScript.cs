using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour, InteractableObject
{
    //TODO
    public bool sheepLock = false; //when enabled, sheep will automatically stop on the elevator and must be dismissed manually
    //TODO
    public bool automatedMovement = false; //when automated, the elevator will move on its own and wont allow manual dragging

    public enum startingPlacement {top, bottom};
    public startingPlacement startingOrientation = startingPlacement.bottom;

    public double shaftDepthUnits = 6; //depth of shaft either height or depth wise depending on starting orientation
    public double unitSnap = 0.5; //the elevator will snap to the nearest this unit

    double shaftPeakY; //maximum Y of the elevator
    double shaftBottomY; //minimum Y of the elevator

    bool currentlyMouseBound = false;
    float speedMult = 8;

    // Start is called before the first frame update
    void Start()
    {
        //register shaft limits
        if (startingOrientation == startingPlacement.bottom)
        {
            shaftBottomY = gameObject.transform.position.y;
            shaftPeakY = shaftBottomY + shaftDepthUnits;
        }
        else
        {
            shaftPeakY = gameObject.transform.position.y;
            shaftBottomY = shaftPeakY - shaftDepthUnits;
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
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mouseDirection = new Vector2(mousePos.x - gameObject.transform.position.x, mousePos.y - gameObject.transform.position.y);
                mouseDirection.Normalize();
                float applicableMult = speedMult;
                //Debug.Log("Vec to Mouse: "+mouseDirection);
                if((gameObject.transform.position.y < shaftBottomY && mouseDirection.y < 0) || (gameObject.transform.position.y > shaftPeakY && mouseDirection.y > 0)) applicableMult = 0;
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
        double snappedHeight = gameObject.transform.position.y;
        if(gameObject.transform.position.y > shaftPeakY) snappedHeight = shaftPeakY; else if(gameObject.transform.position.y < shaftBottomY) snappedHeight = shaftBottomY;
        else
        {
            double relativeY = (gameObject.transform.position.y - shaftBottomY); // height on a scale if the bottom of the shaft were 0
            snappedHeight = Mathf.Round((float)(relativeY/unitSnap))*unitSnap + shaftBottomY; //snapping to the nearest unitsnap unit
            if(snappedHeight > shaftPeakY) snappedHeight = shaftPeakY; else if (snappedHeight < shaftBottomY) snappedHeight = shaftBottomY; //check odd rounding errors from depth / snap unit mismatches
        }
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        gameObject.transform.Translate(new Vector3(0, (float)(snappedHeight - gameObject.transform.position.y), 0));
    }

    public bool onInteract()
    {
        currentlyMouseBound = true;
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        //Debug.Log("Elevator interact script successfully procced.");
        return true;
    }
}
