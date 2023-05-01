using CoolFramework.Core;
using TMPro;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class ScoreManager : CoolSingleton<ScoreManager>, IUpdate
{
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init | UpdateRegistration.Update;

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Image rankImage;
    [SerializeField] private SpriteLibraryAsset rankLibrary;
    [SerializeField] private float scoreDisplaySpeed = 2;
    [SerializeField] private int comboRequirementCD;
    [SerializeField] private int comboRequirementBD;
    [SerializeField] private int comboRequirementAD;
    [SerializeField] private int comboRequirementSD;
    [SerializeField] private int comboRequirementSSD;
    [SerializeField] private int comboRequirementDK;
    private int playerScore = 0;
    private float displayedScore = 0;
    private Ranks currentRank = Ranks.DD;
    private int currentCombo = 0;

    private void Start()
    {
        rankImage.sprite = rankLibrary.GetSprite("Rank", currentRank.ToString());
    }

    void IUpdate.Update()
    {
        displayedScore = Mathf.MoveTowards(displayedScore, playerScore, Time.deltaTime * scoreDisplaySpeed);

        scoreText.text = ((int)displayedScore).ToString();
    }


    public void AddScore(int _amount)
    {
        playerScore = (int)Mathf.Clamp(playerScore + _amount, 0, Mathf.Infinity);
    }
    public void RemoveScore(int _amount)
    {
        playerScore = (int)Mathf.Clamp(playerScore - _amount, 0, Mathf.Infinity);
    }

    public void AugmentCombo(int _amount)
    {
        if (currentRank == Ranks.DK)
            return;

        currentCombo += _amount;

        switch (currentRank)
        {
            case Ranks.DD:
                if(currentCombo >= comboRequirementCD)
                {
                    currentRank = Ranks.CD;
                    currentCombo = 0;
                }
                break;
            case Ranks.CD:
                if (currentCombo >= comboRequirementBD)
                {
                    currentRank = Ranks.BD;
                    currentCombo = 0;
                }
                break;
            case Ranks.BD:
                if (currentCombo >= comboRequirementAD)
                {
                    currentRank = Ranks.AD;
                    currentCombo = 0;
                }
                break;
            case Ranks.AD:
                if (currentCombo >= comboRequirementSD)
                {
                    currentRank = Ranks.SD;
                    currentCombo = 0;
                }
                break;
            case Ranks.SD:
                if (currentCombo >= comboRequirementSSD)
                {
                    currentRank = Ranks.SSD;
                    currentCombo = 0;
                }
                break;
            case Ranks.SSD:
                if (currentCombo >= comboRequirementDK)
                {
                    currentRank = Ranks.DK;
                    currentCombo = 0;
                }
                break;
        }

        rankImage.sprite = rankLibrary.GetSprite("Rank", currentRank.ToString());
    }

    public void BreakCombo()
    {
        currentCombo = 0;

        if(currentRank != Ranks.DD)
        {
            currentRank = (Ranks)((int)currentRank - 1);
            rankImage.sprite = rankLibrary.GetSprite("Rank", currentRank.ToString());
        }
    }
}

public enum Ranks
{
    DD,
    CD,
    BD,
    AD,
    SD,
    SSD,
    DK
}
