using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCaller : MonoBehaviour
{
    public void Playsound(string _name)
    {
        SoundManager.Instance.PlaySound(_name, this.gameObject);
    }
}
