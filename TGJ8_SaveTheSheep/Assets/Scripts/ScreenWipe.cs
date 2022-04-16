using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScreenWipe : MonoBehaviour
{
    public Image screenWipe;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ScreenWipeBlack(Vector3 start, Vector3 target, float duration)
    {
        StartCoroutine(ScreenWiper(start, target, duration));
    }

    public void ScreenWipeVisible(Vector3 start, Vector3 target, float duration)
    {
        StartCoroutine(ScreenWiper(start, target, duration));
    }

    IEnumerator ScreenWiper(Vector3 startVec, Vector3 target, float duration)
    {
        float time = 0;

        while(time < duration)
        {
            transform.localScale = Vector3.Lerp(startVec, target, time / duration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }
        transform.localScale = target;
    }
}
