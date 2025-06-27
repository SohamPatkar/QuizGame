using System;
using UnityEngine;

public enum SoundType
{
    RIGHTANSWER,
    WRONG
}

[Serializable]
public class BackgroundMusicManager
{
    public AudioClip aClip;
    public SoundType soundType;
}
