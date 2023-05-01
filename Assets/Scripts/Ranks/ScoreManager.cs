using CoolFramework.Core;
using TMPro;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class ScoreManager : CoolSingleton<ScoreManager>, IUpdate
{
    public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init | UpdateRegistration.Update;

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private Animator fireAnimator;
    [SerializeField] private SpriteLibraryAsset rankLibrary;
    [SerializeField] private float scoreDisplaySpeed = 2;
    [SerializeField] private int comboRequirementCD;
    [SerializeField] private int comboRequirementBD;
    [SerializeField] private int comboRequirementAD;
    [SerializeField] private int comboRequirementSD;
    [SerializeField] private int comboRequirementSSD;
    [SerializeField] private int comboRequirementDK;
    [SerializeField] private Color DDColor;
    [SerializeField] private Color CDColor;
    [SerializeField] private Color BDColor;
    [SerializeField] private Color ADColor;
    [SerializeField] private Color SDColor;
    [SerializeField] private Color SSDColor;
    [SerializeField] private Color DKColor;
    private int playerScore = 0;
    private float displayedScore = 0;
    public Ranks CurrentRank { get; private set; } = Ranks.DD;
    private int currentCombo = 0;

    private void Start()
    {
        UpdateRankText();

        InvokeRepeating("Temp", 0, .2f);
    }

    void IUpdate.Update()
    {
        displayedScore = Mathf.MoveTowards(displayedScore, playerScore, Time.deltaTime * scoreDisplaySpeed);

        scoreText.text = ((int)displayedScore).ToString();
    }

    void Temp()
    {
        AugmentCombo(4);
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
        if (CurrentRank == Ranks.DK)
            return;

        currentCombo += _amount;

        switch (CurrentRank)
        {
            case Ranks.DD:
                if(currentCombo >= comboRequirementCD)
                {
                    CurrentRank = Ranks.CD;
                    currentCombo = 0;
                    Jenny.Instance.PlayRankAnimation(CurrentRank);
                }
                break;
            case Ranks.CD:
                if (currentCombo >= comboRequirementBD)
                {
                    CurrentRank = Ranks.BD;
                    currentCombo = 0;
                    Jenny.Instance.PlayRankAnimation(CurrentRank);
                }
                break;
            case Ranks.BD:
                if (currentCombo >= comboRequirementAD)
                {
                    CurrentRank = Ranks.AD;
                    currentCombo = 0;
                    Jenny.Instance.PlayRankAnimation(CurrentRank);
                }
                break;
            case Ranks.AD:
                if (currentCombo >= comboRequirementSD)
                {
                    CurrentRank = Ranks.SD;
                    currentCombo = 0;
                    Jenny.Instance.PlayRankAnimation(CurrentRank);
                }
                break;
            case Ranks.SD:
                if (currentCombo >= comboRequirementSSD)
                {
                    fireAnimator.gameObject.SetActive(true);
                    fireAnimator.SetTrigger("SSD");
                    CurrentRank = Ranks.SSD;
                    currentCombo = 0;
                    Jenny.Instance.PlayRankAnimation(CurrentRank);
                }
                break;
            case Ranks.SSD:
                if (currentCombo >= comboRequirementDK)
                {
                    fireAnimator.SetTrigger("DK");
                    CurrentRank = Ranks.DK;
                    currentCombo = 0;
                    Jenny.Instance.PlayRankAnimation(CurrentRank);
                }
                break;
        }

        UpdateRankText();
    }

    public void BreakCombo()
    {
        currentCombo = 0;

        if(CurrentRank != Ranks.DD)
        {
            CurrentRank = (Ranks)((int)CurrentRank - 1);

            if(CurrentRank == Ranks.SSD)
                fireAnimator.SetTrigger("SSD");
            else
                fireAnimator.gameObject.SetActive(false);

            rankText.text = CurrentRank.ToString();
            Jenny.Instance.PlayFailAnimation();
        }
    }

    private void UpdateRankText()
    {
        rankText.text = CurrentRank.ToString();

        switch (CurrentRank)
        {
            case Ranks.DD:
                rankText.color = DDColor;
                break;
            case Ranks.CD:
                rankText.color = CDColor;
                break;
            case Ranks.BD:
                rankText.color = BDColor;
                break;
            case Ranks.AD:
                rankText.color = ADColor;
                break;
            case Ranks.SD:
                rankText.color = SDColor;
                break;
            case Ranks.SSD:
                rankText.color = SSDColor;
                break;
            case Ranks.DK:
                rankText.color = DKColor;
                break;
            default:
                break;
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
