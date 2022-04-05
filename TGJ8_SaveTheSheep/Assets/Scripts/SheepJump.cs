using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class SheepJump : MonoBehaviour
{
    //[Header("")]
    [SerializeField]
    private GameObject sheep;

    [SerializeField]
    private SheepBeh sheepBeh;

    [SerializeField]
    private BoxCollider2D lowObjectCollider;

    [SerializeField]
    private float jumpHeight;

    [SerializeField]
    private int numJump;

    [SerializeField]
    private float jumpDuration;

    [SerializeField]
    private bool isSnapping;

    [SerializeField]
    private float sheepXPosition;

    [SerializeField]
    private float sheepYPosition;

    [SerializeField]
    private float endSheepJumpXValue;


    void Start()
    {
        
    }

    void Update()
    {
        Debug.Log(sheepBeh.CurDir);

        float currentXOffset = lowObjectCollider.offset.x;
        float currentYOffset = lowObjectCollider.offset.y;

        //The collider should be flip to the other side when sheep turn

        /*if (sheepBeh.CurDir == SheepBeh.dir.right)
        {
            //lowObjectCollider.offset = new Vector2(currentXOffset , currentYOffset);
        }

        else if (sheepBeh.CurDir == SheepBeh.dir.left)
        {
            //lowObjectCollider.offset = new Vector2(currentXOffset * -1 , currentYOffset);
        }*/

    }

    //Issue - If LowObjCollider is too high, sheepBeh will count it as a wall.
    //Causing Sheep to constantly flip as if hitting a wall
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Sheep detects low object " + collision.name);
        sheepYPosition = sheep.transform.position.y;
        sheepXPosition = sheep.transform.position.x;

        if (!collision.gameObject.CompareTag("Floor"))
        {
            sheep.transform.DOJump(new Vector2(sheepXPosition + endSheepJumpXValue , sheepYPosition) , jumpHeight , numJump , jumpDuration , isSnapping);
        }
 
    }
}