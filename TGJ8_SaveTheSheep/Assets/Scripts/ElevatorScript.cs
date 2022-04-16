using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour, InteractableObject
{
    public bool sheepLock = false; //when enabled, sheep will automatically stop on the elevator and must be dismissed manually
    public bool automatedMovement = false; //when automated, the elevator will move on its own and wont allow manual dragging
    bool automatedMovement_curGoingUpShaft = false;
    bool automatedMovement_waitingForTurn = false;
    public float automatedMovement_waitPeriod = 1f;
    public float automatedMovement_moveSpeed = 4f;

    public enum orientation {vertical, horizontal};
    public enum startingPlacement {top, bottom, left, right};
    public startingPlacement startingLocale = startingPlacement.bottom;
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
        //register shaft limits
        if (startingLocale == startingPlacement.bottom)
        {
            shaftBottom = gameObject.transform.position.y;
            shaftPeak = shaftBottom + shaftDepthUnits;
            curOrientation = orientation.vertical;
        }
        else if (startingLocale == startingPlacement.top)
        {
            shaftPeak = gameObject.transform.position.y;
            shaftBottom = shaftPeak - shaftDepthUnits;
            curOrientation = orientation.vertical;
        }
        else if (startingLocale == startingPlacement.left)
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
        if(automatedMovement) unlockMovement(); else lockMovement();
    }

    // Update is called once per frame
    void Update()
    {
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
                //Debug.Log("Vec to Mouse: "+mouseDirection);
                if((curOrientation == orientation.vertical && ((gameObject.transform.position.y < shaftBottom && mouseDirection.y < 0) || (gameObject.transform.position.y > shaftPeak && mouseDirection.y > 0))) ||
                (curOrientation == orientation.horizontal && ((gameObject.transform.position.x < shaftBottom && mouseDirection.x < 0) || (gameObject.transform.position.x > shaftPeak && mouseDirection.x > 0))) ||
                (Mathf.Abs(mousePos.x - gameObject.transform.position.x) < followDeadzone && Mathf.Abs(mousePos.y - gameObject.transform.position.y) < followDeadzone)) applicableMult = 0;
                gameObject.GetComponent<Rigidbody2D>().velocity = mouseDirection * applicableMult;
            }
        }
        else if(automatedMovement && !automatedMovement_waitingForTurn)
        {
            //Debug.Log("Flag Aut.a");
            double currentPos = curOrientation == orientation.vertical ? gameObject.transform.position.y : gameObject.transform.position.x;
            if(currentPos < shaftBottom && automatedMovement_curGoingUpShaft == false)
            {
                //Debug.Log("Flag Aut.b1");
                gameObject.transform.Translate(curOrientation == orientation.vertical ? new Vector3(0, (float)(shaftBottom - currentPos), 0) : new Vector3((float)(shaftBottom - currentPos), 0, 0));
                automatedMovement_waitingForTurn = true;
            }
            else if(currentPos > shaftPeak && automatedMovement_curGoingUpShaft == true)
            {
                //Debug.Log("Flag Aut.b2");
                gameObject.transform.Translate(curOrientation == orientation.vertical ? new Vector3(0, (float)(shaftPeak - currentPos), 0) : new Vector3((float)(shaftPeak - currentPos), 0, 0));
                automatedMovement_waitingForTurn = true;
            }
            else
            {
                //Debug.Log("Flag Aut.b3");
                gameObject.GetComponent<Rigidbody2D>().velocity = (curOrientation == orientation.vertical ? (automatedMovement_curGoingUpShaft ? Vector2.up : Vector2.down) : (automatedMovement_curGoingUpShaft ? Vector2.right : Vector2.left)) * automatedMovement_moveSpeed;
            }
            if(automatedMovement_waitingForTurn) //if reached track end, queue turn and wait coroutine
            {
                //Debug.Log("Flag Aut.c");
                StartCoroutine(automatedMoveWaitForTurn());
            }
        }

        if(sheepLock) scanForSheep();
    }

    void scanForSheep()
    {
        float maxScanDistance = 0.6f; // distance to scan up from the origin of the "elevator"
        List<RaycastHit2D> hitList = new List<RaycastHit2D>();
        if (Physics2D.Raycast(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), Vector2.up, _KiroLib.getSheepFilter(), hitList, maxScanDistance) > 0)
        hitList[0].collider.gameObject.GetComponent<SheepBeh>().askToWait();
    }

    IEnumerator automatedMoveWaitForTurn()
    {
        //Debug.Log("Flag Aut.d");
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        lockMovement();
        yield return new WaitForSecondsRealtime(automatedMovement_waitPeriod);
        automatedMovement_curGoingUpShaft = !automatedMovement_curGoingUpShaft;
        unlockMovement();
        automatedMovement_waitingForTurn = false;
        //Debug.Log("Flag Aut.e");
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
        lockMovement();
        if(curOrientation == orientation.vertical) gameObject.transform.Translate(new Vector3(0, (float)(snappedHeight - gameObject.transform.position.y), 0));
        else gameObject.transform.Translate(new Vector3((float)(snappedHeight - gameObject.transform.position.x), 0, 0));
    }

    public bool onInteract()
    {
        if(!automatedMovement)
        {
            currentlyMouseBound = true;
            unlockMovement();
            //Debug.Log("Elevator interact script successfully procced.");
            return true;
        } else return false;
    }

    void lockMovement() //locks elevator movement
    {
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }

    void unlockMovement() //unlocks elevator movement
    {
        if(curOrientation == orientation.vertical) gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        else gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
    }
}
