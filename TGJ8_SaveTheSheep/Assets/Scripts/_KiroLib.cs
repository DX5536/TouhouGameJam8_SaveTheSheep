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

    public static ContactFilter2D getSheepFilter()
    {
        ContactFilter2D sheepFilter = new ContactFilter2D();
        sheepFilter.useLayerMask = true;
        sheepFilter.layerMask = LayerMask.GetMask("Sheep");
        return sheepFilter;
    }
    
    public static ContactFilter2D getDBulletFilter()
    {
        ContactFilter2D sheepFilter = new ContactFilter2D();
        sheepFilter.useLayerMask = true;
        sheepFilter.layerMask = LayerMask.GetMask("Deflectable_Bullet");
        return sheepFilter;
    }
}
