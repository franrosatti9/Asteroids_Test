using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "SO/Sound Effect", fileName = "New Sound Effect")]
public class SoundEffectSO : ScriptableObject
{
    [SerializeField] SFXType type;
    public SFXType Type => type;
    [SerializeField] AudioClip[] clips;

    public AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }
}
