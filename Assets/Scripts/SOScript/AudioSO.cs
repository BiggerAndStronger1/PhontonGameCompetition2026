using UnityEngine;
using System;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "AudioSO", menuName = "Scriptable Objects/AudioSO")]
public class AudioSO : ScriptableObject
{
    public AudioClip[] clipList;
}
