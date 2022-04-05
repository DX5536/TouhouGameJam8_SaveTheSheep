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
    private float endSheepJumpValue;


    void Start()
    {
        
    }

    void Update()
    {
        
        
    }

    //Issue - If LowObjCollider is too high, sheepBeh will count it as a wall.
    //Causing Sheep to constantly flip as if hitting a wall
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Sheep detects low object" + collision.name);
        sheepYPosition = sheep.transform.position.y;
        sheepXPosition = sheep.transform.position.x;

        sheep.transform.DOJump(new Vector2(sheepXPosition + endSheepJumpValue, sheepYPosition) , jumpHeight , numJump , jumpDuration , isSnapping);
    }
}