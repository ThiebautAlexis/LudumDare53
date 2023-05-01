using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : Trigger
{
    [SerializeField] private DialogueData dialogue;

    public override void OnEnter(Movable _movable)
    {
        if(_movable.TryGetComponent(out PlayerController _playerController))
        {
            DialogueBox.Instance.ReadText(dialogue);
            gameObject.SetActive(false);
        }
    }
}
