using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeHandler : MonoBehaviour
{
    float startingXVel = -4f;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(startingXVel, 0);
        if(startingXVel > 0) gameObject.GetComponent<SpriteRenderer>().flipX = true;
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
        collideables.layerMask = LayerMask.GetMask("Sheep", "Default");

        List<RaycastHit2D> hitList = new List<RaycastHit2D>();
        if(Physics2D.Raycast(new Vector2(gameObject.GetComponent<Rigidbody2D>().position.x + (startingXVel > 0 ? -0.1f : 0.1f), gameObject.GetComponent<Rigidbody2D>().position.y), startingXVel > 0 ? Vector2.right : Vector2.left, collideables, hitList, 0.45f) > 0)
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
}
