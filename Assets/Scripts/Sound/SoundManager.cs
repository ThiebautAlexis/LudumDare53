using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private SoundDatabase soundDatabase;
    public SoundDatabase SoundDatabase { get { return soundDatabase; } }


    private void Start()
    {
        Instance = this;
    }

    public void PlaySound(string _name, GameObject _parent)
    {
        Sound _sound = soundDatabase.GetClip(_name);

        if (_sound == null)
            return;

        AudioSource _source = _parent.AddComponent<AudioSource>();
        _source.clip = _sound.Clips[Random.Range(0, _sound.Clips.Count)];
        _source.loop = _sound.Loop;
        _source.volume = _sound.Volume;
        _source.outputAudioMixerGroup = _sound.MixerGroup;
        _source.Play();

        SoundKiller _killer = _parent.AddComponent<SoundKiller>();
        _killer.InitKiller(false, _source);
    }

    public void PlaySound(string _name, Vector3 _position)
    {
        Sound _sound = soundDatabase.GetClip(_name);

        if (_sound == null)
            return;

        GameObject _parent = new GameObject();
        _parent.name = _name + " sound";

        AudioSource _source = _parent.AddComponent<AudioSource>();
        _source.clip = _sound.Clips[Random.Range(0, _sound.Clips.Count)];
        _source.loop = _sound.Loop;
        _source.volume = _sound.Volume;
        _source.outputAudioMixerGroup = _sound.MixerGroup;
        _source.Play();

        SoundKiller _killer = _parent.AddComponent<SoundKiller>();
        _killer.InitKiller(true, _source);
    }
}
