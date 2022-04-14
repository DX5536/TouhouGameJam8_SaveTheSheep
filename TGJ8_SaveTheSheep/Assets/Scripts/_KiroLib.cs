using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _KiroLib : MonoBehaviour
{
    //returns whether the raycast hits collision within the detectionrange
    public static bool raycastToWall(Vector2 origin, Vector2 dir, double detectionRange)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, dir);
        //Debug.DrawRay(hit.point , origin, Color.red);
        if(hit.collider != null)
        {
            return hit.distance < detectionRange;
        }
        else return false;
    }
}
