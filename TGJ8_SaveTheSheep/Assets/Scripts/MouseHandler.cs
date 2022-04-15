using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{
    public AudioSource sfxClickNoClickable;
    public AudioSource sfxClickSheepTurn;
    public AudioSource sfxClickSheepJump;
    public AudioSource sfxClickDestroyProjectile;
    public AudioSource sfxClickInteractableTerrain;
    public Animator anim;

    // Update is called once per frame
    void Update()
    {
        //sheep jumps when left click
        if(Input.GetMouseButtonDown(0))
        {
            anim.SetBool("Pressed", true);
            Collider2D sheepColl = detectSheepAtCurs();
            if(sheepColl != null)
            {
                sheepColl.gameObject.GetComponent<SheepBeh>().mouseJump();
                sfxClickSheepJump.Play();
            }
            else
            {
                Collider2D dBullColl = detectDestructableBulletAtCurs();
                if(dBullColl != null)
                {
                    dBullColl.gameObject.GetComponent<KnifeHandler>().clickDestroy();
                    sfxClickDestroyProjectile.Play();
                }
                else
                {
                    Collider2D intObjColl = detectInteractableObject();
                    if(intObjColl != null)
                    {
                        intObjColl.gameObject.GetComponent<InteractableObject>().onInteract();
                        sfxClickInteractableTerrain.Play();
                    }
                    else sfxClickNoClickable.Play();
                }
            }
        }
        //turn sheep if right click
        else if (Input.GetMouseButtonDown(1))
        {
            anim.SetBool("Pressed", true);
            Collider2D sheepColl = detectSheepAtCurs();
            if(sheepColl != null)
            {
                sheepColl.gameObject.GetComponent<SheepBeh>().mouseTurn();
                sfxClickSheepTurn.Play();
            }
        }
        if(!Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {
            anim.SetBool("Pressed", false);
        }
    }

    Collider2D detectSheepAtCurs()
    {
        List<RaycastHit2D> hitList = new List<RaycastHit2D>();
        if (Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, _KiroLib.getSheepFilter(), hitList, 0.1f) > 0)
        {
            return hitList[0].collider;
        }
        else return null;
    }

    Collider2D detectDestructableBulletAtCurs()
    {
        List<RaycastHit2D> hitList = new List<RaycastHit2D>();
        if (Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, _KiroLib.getDBulletFilter(), hitList, 0.1f) > 0)
        {
            return hitList[0].collider;
        }
        else return null;
    }

    Collider2D detectInteractableObject()
    {
        List<RaycastHit2D> hitList = new List<RaycastHit2D>();
        if (Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, _KiroLib.getInteractableObjectFilter(), hitList, 0.1f) > 0)
        {
            return hitList[0].collider;
        }
        else return null;
    }

}
