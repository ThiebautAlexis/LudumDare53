using CoolFramework.Core;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Jenny : CoolSingleton<Jenny>
{
    [SerializeField] private SpriteLibraryAsset jennyReactionsLibrary;
    [SerializeField] private float reactionDurationInSeconds = 1.5f;

    public void PlayRankAnimation(Ranks _rank)
    {
        TalkieWalkie.Instance.Display(jennyReactionsLibrary.GetSprite("Reaction", _rank.ToString()), reactionDurationInSeconds);
        SoundManager.Instance.PlaySound($"Rang {_rank.ToString()}", gameObject); 
    }

    public void PlayFailAnimation()
    {
        TalkieWalkie.Instance.Display(jennyReactionsLibrary.GetSprite("Reaction", "Fail"), reactionDurationInSeconds);
    }
}
