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

    private void OnEnable()
    {
        GameManager.enter += ScreenWipeVisible;
        GameManager.exit += ScreenWipeBlack;
    }

    private void OnDisable()
    {
        GameManager.enter -= ScreenWipeVisible;
        GameManager.exit -= ScreenWipeBlack;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ScreenWipeBlack()
    {
        StartCoroutine(ScreenWiper(new Vector3(0f, 0f, 0f), new Vector3(20f, 20f, 0f), 0.5f));
    }

    public void ScreenWipeVisible()
    {
        StartCoroutine(ScreenWiper(new Vector3(20f, 20f, 0f), new Vector3(0f, 0f, 0f), 0.5f));
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
