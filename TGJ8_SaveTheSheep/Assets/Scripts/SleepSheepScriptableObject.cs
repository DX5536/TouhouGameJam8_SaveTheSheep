using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SleepSheepScriptableObject", order = 1)]
public class SleepSheepScriptableObject : ScriptableObject
{
    public bool needDialogue;
    public AudioClip playingAudio;
    public AudioClip pauseAudio;
}
