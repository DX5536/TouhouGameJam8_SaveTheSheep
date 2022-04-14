using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepBeh : MonoBehaviour
{

    public enum dir {left, right}

    //assuming this direction's default is the default direction of the sheep sprite
    public dir startdir = dir.right;

    double moveSpeed = 1.5;
    Animator anim;
    double wallDetectionRange = 0.05;
    float wallDeadzone = 0.501f;
    float wallVertCheck= 0.15f;
    double floorDetectionRange = 0.05;
    float floorDeadzone = 0.501f;
    float fatalFallDistance = 2.95f;
    float floorOffsetChecks = Mathf.Sqrt(2)/4;
    bool hasJumped = false;
    bool inGroundCheckLoop = false;
    bool currentlyTrackingFall = false;
    Vector2 trackingFallOrigin;
    float JumpVelocity = 7f;


    //determines the maximum velocity down a sheep may go before their forward momentum halts
    double maxFallVBeforeHalt = 0.5;
    dir curDir;
    double curXVel;

    public dir CurDir
    {
        get { return curDir; }
        set { curDir = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        gameObject.GetComponent<SpriteRenderer>().flipX = startdir != dir.right ? true : false;
        curDir = startdir;
        curXVel = startdir == dir.right ? moveSpeed : -moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        bool isFall = gameObject.GetComponent<Rigidbody2D>().velocity.y < -maxFallVBeforeHalt;
        anim.SetBool("Falling", isFall);
        if(isFall && !currentlyTrackingFall)
        {
            currentlyTrackingFall = true;
            trackingFallOrigin = gameObject.GetComponent<Rigidbody2D>().position;
        }
        if(isFall && !hasJumped)
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gameObject.GetComponent<Rigidbody2D>().velocity.y);
        else
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2((float)curXVel, gameObject.GetComponent<Rigidbody2D>().velocity.y);
            if (detectWall()) flip();
            if(hasJumped && isFall && !inGroundCheckLoop) inGroundCheckLoop = true;
            if(inGroundCheckLoop && isGrounded())
            {
                hasJumped = false;
                inGroundCheckLoop = false;

            }
        }
        if(currentlyTrackingFall && !isFall)
        {
            currentlyTrackingFall = false;
            if(isFatalFall(trackingFallOrigin, gameObject.GetComponent<Rigidbody2D>().position)) killByFall();
        }

    }

    void flip()
    {
        //Debug.Log("flip has been called");
        curDir = curDir == dir.left ? dir.right : dir.left;
        gameObject.GetComponent<SpriteRenderer>().flipX = curDir != dir.right ? true : false;
        curXVel = curDir == dir.right ? moveSpeed : -moveSpeed;
    }

    bool detectWall()
    {
        bool detectedWall = false;
        float xOffset = transform.position.x + (curDir == dir.right ? wallDeadzone : -wallDeadzone);
        Vector2 rayorigin = new Vector2(xOffset, transform.position.y);

        detectedWall = _KiroLib.raycastToWall(rayorigin, curDir == dir.right ? Vector2.right : Vector2.left, wallDetectionRange);
        //the other checks higher and lower to catch other terrain
        if(!detectedWall)
        {
            Vector2 rayoriginH = new Vector2(xOffset, transform.position.y+wallVertCheck);
            detectedWall = _KiroLib.raycastToWall(rayoriginH, curDir == dir.right ? Vector2.right : Vector2.left, wallDetectionRange);
            if(!detectedWall)
            {
                Vector2 rayoriginL = new Vector2(xOffset, transform.position.y-wallVertCheck);
                detectedWall = _KiroLib.raycastToWall(rayoriginL, curDir == dir.right ? Vector2.right : Vector2.left, wallDetectionRange);
            }
        }
        return detectedWall;
    }

    bool isGrounded()
    {
        //suspected issues when the sheep is on terrain outside the three existing raycasts, for instance when it catches its hitboxes lip onto a surface but doesnt successfully surface. If this becomes an issue more checks will be added
        Vector2 rayorigin = new Vector2(transform.position.x, transform.position.y-floorDeadzone);
        Vector2 leftrayoffset = new Vector2(rayorigin.x - floorOffsetChecks, transform.position.y-(floorOffsetChecks+0.01f));
        Vector2 rightrayoffset = new Vector2(rayorigin.x + floorOffsetChecks, transform.position.y-(floorOffsetChecks+0.01f));
        bool detectedGround = _KiroLib.raycastToWall(rayorigin, Vector2.down, floorDetectionRange);
        if(!detectedGround) detectedGround = _KiroLib.raycastToWall(leftrayoffset, Vector2.down, floorDetectionRange);
        if(!detectedGround) detectedGround = _KiroLib.raycastToWall(rightrayoffset, Vector2.down, floorDetectionRange);
        return detectedGround;
    }

    void doJump()
    {
        //does jump for sheep, goal is 2 tiles high (little more than 2 tiles for room for error)
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(gameObject.GetComponent<Rigidbody2D>().velocity.x, JumpVelocity);
        hasJumped = true;
    }

    bool isFatalFall(Vector2 orig, Vector2 current)
    {
        return orig.y - current.y > fatalFallDistance;
    }

    void killByFall()
    {
        //TODO, kills sheep from fall damage
        //TODO
        Debug.Log("Sheep death by fall damage called");
    }

    void kill()
    {
        //TODO, generic kill handler for sheep
        //TODO
        //Send this death to the script that handles level progress / sheep saved / sheep deaths?
        Debug.Log("Sheep death by generic handler called");
    }


    //externally accessible functions (to be called by mouse handler)
    public void mouseJump()
    {
        doJump();
        //Debug.Log("mouse Jump procced!");
    }

    public void mouseTurn()
    {
        if(isGrounded()) flip();
    }

}
