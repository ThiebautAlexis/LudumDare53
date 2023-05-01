using CoolFramework.Core;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Jenny : CoolSingleton<Jenny>
{
    [SerializeField] private SpriteLibraryAsset jennyReactionsLibrary;
    [SerializeField] TalkieWalkie talkieWalkie;
    [SerializeField] private float reactionDurationInSeconds = 1.5f;

    public void PlayRankAnimation(Ranks _rank)
    {
        talkieWalkie.Display(jennyReactionsLibrary.GetSprite("Reaction", _rank.ToString()), reactionDurationInSeconds);
    }

    public void PlayFailAnimation()
    {
        talkieWalkie.Display(jennyReactionsLibrary.GetSprite("Reaction", "Fail"), reactionDurationInSeconds);
    }
}
