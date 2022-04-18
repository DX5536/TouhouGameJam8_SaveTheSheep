using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyMenuButtons : MonoBehaviour
{
    public Sprite normalText;
    public Sprite highlightedText;
    public Sprite SelectedText;
    SpriteRenderer thisRenderer;
    public enum buttonType {play, credits, quit, credits_window};
    public buttonType thisButtonType = buttonType.play;
    float timeUntilIcoDecay = .2f;
    bool decayed = true;
    bool currentlyInDecayLoop = false;
    // early writing of getting visuals to update before being abandoned
    void Start()
    {
        thisRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    IEnumerator spriteDecayLoop()
    {
        while(!decayed)
        {
            decayed = true;
            yield return new WaitForSecondsRealtime(timeUntilIcoDecay);
        }
        thisRenderer.sprite = normalText;
        currentlyInDecayLoop = false;
    }

    public void triggerClick()
    {
        if(thisButtonType != buttonType.credits_window)
        {
            decayed = false;
            if(!currentlyInDecayLoop)
            {
                currentlyInDecayLoop = true;
                thisRenderer.sprite = SelectedText;
                StartCoroutine(spriteDecayLoop());
            }
        }
    }
}
