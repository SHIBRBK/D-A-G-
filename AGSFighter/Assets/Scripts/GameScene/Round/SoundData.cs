using UnityEngine;

[CreateAssetMenu(fileName = "NewSoundData", menuName = "SoundData")]
public class SoundData : ScriptableObject
{
    public string soundName;
    public AudioClip clip;
}
