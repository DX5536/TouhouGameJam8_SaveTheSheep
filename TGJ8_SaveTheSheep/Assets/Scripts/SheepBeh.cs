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


    //determines the maximum velocity down a sheep may go before their forward momentum halts
    double maxFallVBeforeHalt = 0.5;
    dir curDir;
    double curXVel;

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
        if(isFall)
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gameObject.GetComponent<Rigidbody2D>().velocity.y);
        else
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2((float)curXVel, gameObject.GetComponent<Rigidbody2D>().velocity.y);
            if (detectWall()) flip();
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
        // todo: raycast test for wall
        RaycastHit2D hit;
        float xOffset = transform.position.x + (curDir == dir.right ? wallDeadzone : -wallDeadzone);
        Vector2 rayorigin = new Vector2(xOffset, transform.position.y);
        hit = Physics2D.Raycast(rayorigin, curDir == dir.right ? Vector2.right : Vector2.left);
        //Debug.Log("Sheep origin: "+transform.position.x+", "+transform.position.y);
        //Debug.Log("Ray origin: "+xOffset+", "+transform.position.y);
        //Debug.Log("Wall distance: "+hit.distance);
        //Debug.Log("Wall: "+hit.collider);
        if (hit.collider != null) detectedWall = hit.distance < wallDetectionRange;
        //the other checks higher and lower to catch other terrain
        if(!detectedWall)
        {
            Vector2 rayoriginH = new Vector2(xOffset, transform.position.y+wallVertCheck);
            hit = Physics2D.Raycast(rayoriginH, curDir == dir.right ? Vector2.right : Vector2.left);
            if (hit.collider != null) detectedWall = hit.distance < wallDetectionRange;
            if(detectedWall) return detectedWall; else
            {
                Vector2 rayoriginL = new Vector2(xOffset, transform.position.y-wallVertCheck);
                hit = Physics2D.Raycast(rayoriginL, curDir == dir.right ? Vector2.right : Vector2.left);
                if (hit.collider != null) detectedWall = hit.distance < wallDetectionRange;
            }
        }
        return detectedWall;
    }

    //possible test w/ raycasting to prevent sheep from walking off cliffs of a certain depth?

    //possible test w/ raycasting to make sheep automatically jump if the wall in front of them is short enough to be hopped over?
}
