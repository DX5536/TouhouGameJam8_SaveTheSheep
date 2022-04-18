using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;


public class PlaySpecificAnimationAtStart : MonoBehaviour
{
    [Header("")]
    [SerializeField]
    private string platformAnimationName;

    [SerializeField]
    private Animator platformAnimator;

    void Start()
    {
        platformAnimator.Play(platformAnimationName);
    }

    void Update()
    {
        
    }
}