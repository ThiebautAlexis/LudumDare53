using UnityEngine;
using UnityEngine.UI;
using CoolFramework.Core;
using DG.Tweening; 

public class TalkieWalkie : CoolSingleton<TalkieWalkie>, IUpdate
{
    public override UpdateRegistration UpdateRegistration => base.UpdateRegistration | UpdateRegistration.Update;

    [Header("Character")]
    [SerializeField] private Image characterBubble; 
    [SerializeField] private Image characterImage;
    [SerializeField] private Sprite jennyBubbleSprite;
    [SerializeField] private Sprite supervisorBubbleSprite;

    [Header("Talkie Walkie")]
    [SerializeField] private Transform talkiewalkieTransform;
    [SerializeField] private float shakingDuration = 1.0f;
    [SerializeField] private float shakingForce = 1.0f; 
    [SerializeField] private float shakingVibrato = 1.0f; 

    private float duration = 0f, timer = 0f;
    private bool isDisplaying = false;
    private Sequence shakingSequence = null;


    void IUpdate.Update()
    {
        if (!isDisplaying) return;
        timer += Time.deltaTime;

        if (timer >= duration)
            Hide(); 
    }


    private void Hide()
    {
        isDisplaying = false;
        characterBubble.gameObject.SetActive(false);
        characterImage.sprite = null; 

    }

    public void Display(Sprite _characterSprite, float _duration, bool _isSupervisor = false)
    {
        // Play sound here
        isDisplaying = true;
        characterImage.sprite = _characterSprite;
        characterBubble.gameObject.SetActive(true);
        characterBubble.sprite = _isSupervisor ? supervisorBubbleSprite : jennyBubbleSprite;
        timer = 0f;
        duration = _duration;

        if (shakingSequence.IsActive())
            shakingSequence.Kill(true);

        shakingSequence = DOTween.Sequence();
        shakingSequence.Join(talkiewalkieTransform.DOShakePosition(shakingDuration/2, shakingForce));
        shakingSequence.AppendInterval(.1f); 
        shakingSequence.Append(talkiewalkieTransform.DOShakePosition(shakingDuration/2, shakingForce)); 
    }

}
