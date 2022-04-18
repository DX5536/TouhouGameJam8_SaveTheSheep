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
    public static bool raycastToWall(Vector2 origin, Vector2 dir, ContactFilter2D filter, double detectionRange)
    {
        List<RaycastHit2D> hitList = new List<RaycastHit2D>();
        return Physics2D.Raycast(origin, dir, filter, hitList, (float)detectionRange) > 0;
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
        ContactFilter2D dBulletFilter = new ContactFilter2D();
        dBulletFilter.useLayerMask = true;
        dBulletFilter.layerMask = LayerMask.GetMask("Deflectable_Bullet");
        return dBulletFilter;
    }

    public static ContactFilter2D getDefaultFilter()
    {
        ContactFilter2D defFilter = new ContactFilter2D();
        defFilter.useLayerMask = true;
        defFilter.layerMask = LayerMask.GetMask("Default", "Interactable_Terrain", "Sheep");
        return defFilter;
    }

    public static ContactFilter2D getInteractableObjectFilter()
    {
        ContactFilter2D intFilter = new ContactFilter2D();
        intFilter.useLayerMask = true;
        intFilter.layerMask = LayerMask.GetMask("Interactable_Terrain");
        return intFilter;
    }

    public static ContactFilter2D getPanicFilter() //AAAAAAAAAAAAAAAAHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH AHHHHHHHHHHHHHHHHHHH AHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHAHAHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH
    {
        ContactFilter2D AHHFilter = new ContactFilter2D();
        AHHFilter.useLayerMask = true;
        AHHFilter.layerMask = LayerMask.GetMask("MM_EmergencyLayer");
        return AHHFilter;
    }
}
