using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundKiller : MonoBehaviour
{
    private bool killObject;
    private AudioSource audioSource;

    public event System.Action OnSoundKill = null;

    public void InitKiller(bool _killObject, AudioSource _source)
    {
        killObject = _killObject;
        audioSource = _source;

        if (!audioSource.loop)
            Invoke("Kill", audioSource.clip.length);
    }

    public void Kill()
    {
        if (killObject)
        {
            Destroy(audioSource.gameObject);
        }
        else
        {
            Destroy(audioSource);
        }

        OnSoundKill?.Invoke();
        Destroy(this);
    }
}
