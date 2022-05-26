using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGoalSheepScan : MonoBehaviour
{
    public float scanXOffset = 0f;
    public float scanYOffset = 0f;
    // Update is called once per frame
    void Update()
    {
        List<RaycastHit2D> hitList = new List<RaycastHit2D>();
        if (Physics2D.Raycast(new Vector2(gameObject.transform.position.x + scanXOffset, gameObject.transform.position.y + scanYOffset), Vector2.zero, _KiroLib.getSheepFilter(), hitList, 1) > 0)
        hitList[0].collider.gameObject.GetComponent<SheepBeh>().markAsDelivered();

    }
}
