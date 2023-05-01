using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "GGJ/Dialogue/Data")]
public class DialogueData : ScriptableObject
{
    [SerializeField, TextArea] private string displayedText = string.Empty;
    [SerializeField] private float duration = 5f;
    [SerializeField] private Sprite characterImage; 
    
    public string DisplayedText => displayedText; 
    public float Interval
    {
        get
        {
            if (displayedText.Length == 0 || duration == 0f) return 1f; 
            return duration / displayedText.Length;
        }
    }
    public float Duration => duration; 

    public Sprite ChartacterImage => characterImage; 
}
