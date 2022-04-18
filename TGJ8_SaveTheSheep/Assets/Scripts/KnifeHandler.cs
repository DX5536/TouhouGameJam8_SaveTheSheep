using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeHandler : MonoBehaviour
{
    float startingXVel = -4f;
    bool spawnedByTurret = false;
    float projectileLength = 0.7f; //positive
    float projectileOffsetX = -0.2f;//assuming arrow is travelling forward along x
    // Start is called before the first frame update
    void Start()
    {
        if(!spawnedByTurret) setXVelocity(startingXVel);
    }

    // Update is called once per frame
    void Update()
    {
        //check if there is collision w/ object, if no continue, if yes check if sheep, if sheep tell sheep knife, regardless destroy self on hit
        detectCollision();
    }

    void detectCollision()
    {
        ContactFilter2D collideables = new ContactFilter2D();
        collideables.useLayerMask = true;
        collideables.layerMask = LayerMask.GetMask("Sheep", "Default", "Interactable_Terrain");

        List<RaycastHit2D> hitList = new List<RaycastHit2D>();
        if(Physics2D.Raycast(new Vector2(gameObject.GetComponent<Rigidbody2D>().position.x + (startingXVel > 0 ? projectileOffsetX : -projectileOffsetX), gameObject.GetComponent<Rigidbody2D>().position.y), startingXVel > 0 ? Vector2.right : Vector2.left, collideables, hitList, projectileLength) > 0)
        {
            if(hitList[0].collider.gameObject.layer == LayerMask.NameToLayer("Sheep"))
            {
                hitList[0].collider.gameObject.GetComponent<SheepBeh>().killByBullet();
            }
            destroySelf();
        }
    }

    void destroySelf()
    {
        //destroy this projectile
        Debug.Log("self destruction of bullet activated");
        Destroy(gameObject, 0);
    }

    public void clickDestroy()
    {
        destroySelf();
    }

    public void setXVelocity(float vel)
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(vel, 0);
        if(vel > 0) gameObject.GetComponent<SpriteRenderer>().flipX = true; else gameObject.GetComponent<SpriteRenderer>().flipX = false;
    }
    public void turretSpawn(float vel)
    {
        //Debug.Log("spawn script procced");
        spawnedByTurret = true;
        startingXVel = vel;
        setXVelocity(vel);
        if(vel > 0) gameObject.transform.localScale = Vector3.Scale(gameObject.transform.localScale, new Vector3(-1,1,1));
    }
}
