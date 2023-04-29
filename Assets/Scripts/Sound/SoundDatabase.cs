using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SoundDatabase", menuName = "ScriptableObjects/CreateSoundDatabase", order = 0)]
public class SoundDatabase : ScriptableObject
{
    [SerializeField] private List<Sound> allSounds = new List<Sound>();

    public Sound GetClip(string _name)
    {
        return allSounds.Find(s => s.name == _name);
    }

}

[System.Serializable]
public class Sound
{
    public string name;

    public List<AudioClip> Clips = new List<AudioClip>();

    [Range(0f, 1f)]
    public float Volume = 1;

    public bool Loop;
    public AudioMixerGroup MixerGroup;

    public AudioClip GetRandomClip()
    {
        return Clips[Random.Range(0, Clips.Count)];
    }
}
