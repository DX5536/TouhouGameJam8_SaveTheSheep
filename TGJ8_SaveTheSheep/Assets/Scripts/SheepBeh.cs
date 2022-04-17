using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;
using System.Linq;

public class SheepBeh : MonoBehaviour
{
    public enum health { alive, dead, delivered };
    public enum dir {left, right}
    public health healthState = health.alive;
    //assuming this direction's default is the default direction of the sheep sprite
    public dir startdir = dir.right;

    double moveSpeed = 1.5;
    double jumpMult = 2.5;
    Animator anim;
    double wallDetectionRange = 0.05;
    float wallDeadzone = 0.501f;
    float wallVertCheckDown= 0.15f;
    float wallVertCheckUp = 0.499f;
    double floorDetectionRange = 0.05;
    float floorDeadzone = 0.501f;
    float fatalFallDistance = 2.95f;
    float floorOffsetChecks = Mathf.Sqrt(2)/4;
    bool hasJumped = false;
    bool inGroundCheckLoop = false;
    bool currentlyTrackingFall = false;
    Vector2 trackingFallOrigin;
    float JumpVelocity = 5.2f;
    //7f for making 2 block jumps, ~5.2f for 1 block
    bool isWaiting = false;
    bool onWaitCooldown = false;
    float waitingcooldown = 1f;

    public delegate void DeathState();
    public static event DeathState death;

    public delegate void DeliverState();
    public static event DeliverState delivered;
    bool isDelivered = false;
    float jumpCooldownBeforeAntiSlide = 0.5f;
    bool canDoAntislide = false;

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
        wearRandomHat();
        //gameObject.GetComponent<SpriteRenderer>().flipX = startdir != dir.right ? true : false;
        curDir = startdir;
        FlipScale(curDir);
        curXVel = startdir == dir.right ? moveSpeed : -moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthState == health.alive)
        {
            //Debug.Log("is wait "+isWaiting+ " isoncd "+onWaitCooldown);
            bool isFall = (gameObject.GetComponent<Rigidbody2D>().velocity.y < -maxFallVBeforeHalt) && !isGrounded();
            //Debug.Log("fall: "+isFall);
            anim.SetBool("Falling", isFall);
            if (isFall && !currentlyTrackingFall)
            {
                currentlyTrackingFall = true;
                trackingFallOrigin = gameObject.GetComponent<Rigidbody2D>().position;
            }
            if (isFall && !hasJumped)
            {
                if (!isWaiting) gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gameObject.GetComponent<Rigidbody2D>().velocity.y);
            }
            else
            {
                if(hasJumped && canDoAntislide && isGrounded())
                {
                    hasJumped = false;
                    canDoAntislide = false;
                    inGroundCheckLoop = false;
                }
                if (!isWaiting) gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(hasJumped ? (float)(curXVel * jumpMult) : (float)curXVel, gameObject.GetComponent<Rigidbody2D>().velocity.y);
                if (detectWall()) flip();
                if (hasJumped && isFall && !inGroundCheckLoop) inGroundCheckLoop = true;
                if (inGroundCheckLoop && isGrounded())
                {
                    hasJumped = false;
                    canDoAntislide = false;
                    inGroundCheckLoop = false;
                }
            }
            if (currentlyTrackingFall && !isFall)
            {
                currentlyTrackingFall = false;
                if (isFatalFall(trackingFallOrigin, gameObject.GetComponent<Rigidbody2D>().position)) killByFall();
            }
        }
    }

    void flip()
    {
        //Debug.Log("flip has been called");
        curDir = curDir == dir.left ? dir.right : dir.left;
        FlipScale(curDir);
        //gameObject.GetComponent<SpriteRenderer>().flipX = curDir != dir.right ? true : false;
        curXVel = curDir == dir.right ? moveSpeed : -moveSpeed;
    }

    bool detectWall()
    {
        bool detectedWall = false;
        float xOffset = transform.position.x + (curDir == dir.right ? wallDeadzone : -wallDeadzone);
        Vector2 rayorigin = new Vector2(xOffset, transform.position.y);

        detectedWall = _KiroLib.raycastToWall(rayorigin, curDir == dir.right ? Vector2.right : Vector2.left, _KiroLib.getDefaultFilter(), wallDetectionRange);
        //the other checks higher and lower to catch other terrain
        if(!detectedWall)
        {
            Vector2 rayoriginH = new Vector2(xOffset, transform.position.y+wallVertCheckUp);
            detectedWall = _KiroLib.raycastToWall(rayoriginH, curDir == dir.right ? Vector2.right : Vector2.left, _KiroLib.getDefaultFilter(), wallDetectionRange);
            if(!detectedWall)
            {
                Vector2 rayoriginL = new Vector2(xOffset, transform.position.y-wallVertCheckDown);
                detectedWall = _KiroLib.raycastToWall(rayoriginL, curDir == dir.right ? Vector2.right : Vector2.left, _KiroLib.getDefaultFilter(), wallDetectionRange);
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

        bool detectedGround = _KiroLib.raycastToWall(rayorigin, Vector2.down, _KiroLib.getDefaultFilter(), floorDetectionRange);
        if(!detectedGround) detectedGround = _KiroLib.raycastToWall(leftrayoffset, Vector2.down, _KiroLib.getDefaultFilter(), floorDetectionRange);
        if(!detectedGround) detectedGround = _KiroLib.raycastToWall(rightrayoffset, Vector2.down, _KiroLib.getDefaultFilter(), floorDetectionRange);
        return detectedGround;
    }

    void doJump()
    {
        //does jump for sheep, goal is 2 tiles high (little more than 2 tiles for room for error)
        if(!hasJumped)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(gameObject.GetComponent<Rigidbody2D>().velocity.x, JumpVelocity);
            hasJumped = true;
            canDoAntislide = false;
            StartCoroutine(doAntislideTimer());
        }
    }

    IEnumerator doAntislideTimer()
    {
        yield return new WaitForSecondsRealtime(jumpCooldownBeforeAntiSlide);
        if(hasJumped) canDoAntislide = true;
    }

    bool isFatalFall(Vector2 orig, Vector2 current)
    {
        return orig.y - current.y > fatalFallDistance;
    }

    void killByFall()
    {
        //TODO, kills sheep from fall damage
        //TODO
        kill();
        Debug.Log("Sheep death by fall damage called");
    }

    void kill()
    {
        //TODO, generic kill handler for sheep
        //TODO
        //Send this death to the script that handles level progress / sheep saved / sheep deaths?

        //send a death message
        //stop moving
        //call death anims
        //we can either leave it there or clean it up and destroy it
        healthState = health.dead;
        anim.SetBool("Dead", true);
        death();
        StartCoroutine(DeathRoutine(1f, true));

        Debug.Log("Sheep death by generic handler called");
    }

    void deliver()
    {
        //TODO function to facilitate sheep entering the goal
        healthState = health.delivered;
        delivered();
    }


    //externally accessible functions (to be called by mouse handler), now also handles suspending a sheep's wait state
    public void mouseJump()
    {
        if(isWaiting) stopWaiting();
        else doJump();
    }

    void stopWaiting()
    {
        isWaiting = false;
        anim.SetBool("isWaiting", false);
        StartCoroutine(waitingCooldownRoutine());
    }

    public void mouseTurn()
    {
        if(isGrounded()) flip();
    }
    
    public void killByBullet()
    {
        //TODO
        //TODO
        Debug.Log("Sheep death by bullet called");
    }

    public bool askToWait()
    {
        if(isWaiting || onWaitCooldown || !isGrounded()) return false; //waiting request does not go through
        else
        {
            isWaiting = true;
            onWaitCooldown = true;
            anim.SetBool("isWaiting", true);
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gameObject.GetComponent<Rigidbody2D>().velocity.y);
            //waiting cooldown routine expirary routine will be procced on waiting stopping, like when the sheep is clicked so, time until click + cooldown time
            return true;
        }
    }

    IEnumerator waitingCooldownRoutine()
    {
        yield return new WaitForSecondsRealtime(waitingcooldown);
        onWaitCooldown = false;
    }

    private IEnumerator DeathRoutine(float duration, bool flash)
    {
        for (float i = 0; i < duration; i += (0.1f))
        {
            if (flash)
            {
                if (gameObject.GetComponent<SpriteRenderer>().color.a == 255f)
                {
                    SetRendererTo(new Color(255f, 255f, 255f, 0f));

                }
                else
                {
                    SetRendererTo(new Color(255f, 255f, 255f, 255f));


                }
            }
            yield return new WaitForSeconds(0.15f);
        }

        SetRendererTo(new Color(255f, 255f, 255f, 0f));
        Destroy(gameObject);
    }

    private void SetRendererTo(Color col)
    {
        gameObject.GetComponent<SpriteRenderer>().color = col;
    }

    public void markAsDelivered()
    {
        if(!isDelivered)
        {
            isDelivered = true;
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            anim.SetBool("isDelivered", true);
            gameObject.layer = LayerMask.NameToLayer("Sheep_removedFromPlay");
            StartCoroutine(deliveryRoutine());
        }
    }

    IEnumerator deliveryRoutine()
    {
        float winAnimationTime = 1f;

        float time = 0;
        Vector3 startValue = gameObject.transform.localPosition;
        Vector3 targetValue = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2f, gameObject.transform.position.z);

        Color startCol = new Color(1f, 1f, 1f, 1f);
        Color endCol = new Color(1f, 1f, 1f, 0f);

        SpriteRenderer accessory = gameObject.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
        SpriteRenderer backLeg1 = gameObject.transform.GetChild(3).GetChild(0).GetComponent<SpriteRenderer>();
        SpriteRenderer backLeg2 = gameObject.transform.GetChild(3).GetChild(1).GetComponent<SpriteRenderer>();
        SpriteRenderer frontLeg1 = gameObject.transform.GetChild(3).GetChild(2).GetComponent<SpriteRenderer>();
        SpriteRenderer frontLeg2 = gameObject.transform.GetChild(3).GetChild(3).GetComponent<SpriteRenderer>();
        SpriteRenderer body = gameObject.transform.GetChild(5).GetComponent<SpriteRenderer>();

        while (time < winAnimationTime)
        {
            gameObject.transform.position = Vector3.Lerp(startValue, targetValue, time / winAnimationTime);
            accessory.color = Color.Lerp(startCol, endCol, time / winAnimationTime);
            backLeg1.color = Color.Lerp(startCol, endCol, time / winAnimationTime);
            backLeg2.color = Color.Lerp(startCol, endCol, time / winAnimationTime);
            frontLeg1.color = Color.Lerp(startCol, endCol, time / winAnimationTime);
            frontLeg2.color = Color.Lerp(startCol, endCol, time / winAnimationTime);
            body.color = Color.Lerp(startCol, endCol, time / winAnimationTime);

            time += Time.deltaTime;
            yield return null;
        }
        gameObject.transform.position = targetValue;
         
        //yield return new WaitForSeconds(winAnimationTime);
        deliver(); //finalize delivering
    }

    void FlipScale(dir curDir)
    {
        if(curDir == dir.right)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    void wearRandomHat()
    {
        string[] labels = gameObject.GetComponent<SpriteLibrary>().spriteLibraryAsset.GetCategoryLabelNames("Accessory").ToArray();
        int index = Random.Range(0, labels.Length);
        string label = labels[index];
        var accessory = gameObject.transform.GetChild(0).GetChild(0);
        accessory.GetComponent<SpriteRenderer>().enabled = true;
        accessory.GetComponent<SpriteResolver>().SetCategoryAndLabel("Accessory", label);
    }
}
