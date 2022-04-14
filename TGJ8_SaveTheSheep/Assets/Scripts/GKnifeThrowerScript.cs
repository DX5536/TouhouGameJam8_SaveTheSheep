using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GKnifeThrowerScript : MonoBehaviour
{
    public Rigidbody2D knifeToThrow;
    public float startingVelocity = -4f;
    public float xOffset = 0f;
    public float yOffset = 0f;
    public bool usesAudioSource = false;

    float throwDelays = 1;
    bool breakLoop = false;
    AudioSource selfAudio;

    // Start is called before the first frame update
    void Start()
    {
        if(usesAudioSource) selfAudio = gameObject.GetComponent<AudioSource>();
        StartCoroutine(repeatThrow());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator repeatThrow()
    {
        yield return new WaitForSecondsRealtime(throwDelays);
        while(!breakLoop)
        {
            Rigidbody2D newKnife = Instantiate(knifeToThrow, new Vector3(gameObject.transform.position.x+xOffset, gameObject.transform.position.y+yOffset, gameObject.transform.position.z), Quaternion.Euler(0, 0, 0));
            //Debug.Log("loop stepped");
            newKnife.gameObject.GetComponent<KnifeHandler>().turretSpawn(startingVelocity);
            if(usesAudioSource) selfAudio.Play(1);
            yield return new WaitForSecondsRealtime(throwDelays);
        }
    }


}