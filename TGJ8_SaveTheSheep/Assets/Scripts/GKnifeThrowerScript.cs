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
    public bool shootRight = false;

    public float throwDelays = 5;
    bool breakLoop = false;
    AudioSource selfAudio;

    // Start is called before the first frame update
    void Start()
    {
        if(usesAudioSource) selfAudio = gameObject.GetComponent<AudioSource>();
        StartCoroutine(repeatThrow());
        if(shootRight) gameObject.transform.localScale = Vector3.Scale(gameObject.transform.localScale, new Vector3(-1,1,1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator repeatThrow()
    {
        //yield return new WaitForSecondsRealtime(throwDelays);
        while(!breakLoop)
        {
            Rigidbody2D newKnife = Instantiate(knifeToThrow, new Vector3(gameObject.transform.position.x+(shootRight ? -xOffset : xOffset), gameObject.transform.position.y+yOffset, gameObject.transform.position.z), Quaternion.Euler(0, 0, 0));
            //Debug.Log("loop stepped");
            newKnife.gameObject.GetComponent<KnifeHandler>().turretSpawn(shootRight ? -startingVelocity : startingVelocity);
            if(shootRight) newKnife.gameObject.transform.localScale = Vector3.Scale(newKnife.gameObject.transform.localScale, new Vector3(-1,1,1));
            if(usesAudioSource) selfAudio.Play(1);
            yield return new WaitForSecondsRealtime(throwDelays);
        }
    }


}
