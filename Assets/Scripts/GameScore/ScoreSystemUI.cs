using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameBoard;
using TMPro;
using UnityEngine;
using Utils;

public class ScoreSystemUI : MonoBehaviour
{
    [SerializeField] private float _textUpdateTimeSeconds;
    [SerializeField] private AnimationCurve _textUpdateCurve;

    [SerializeField] private TextMeshProUGUI _label;
    [SerializeField] Board board;


    public void SetScore(int score)
    {
        UpdateText(score);
    }

    public UniTask UpdateScoreWithAnimation(int score, int prevScore)
    {
        Debug.Log("Inside score animation");
        var tween = DOTween.To(() => prevScore, UpdateText, score, _textUpdateTimeSeconds);
        tween.OnComplete(() => ShowInterstitial(score));
        tween.SetEase(_textUpdateCurve);
        return tween.PlayAsync();
    }

    private void UpdateText(int score)
    {
        _label.text = score.ToString();
    }

    void ShowInterstitial(int score)
    {
        Debug.Log("Its here in score script : " + " " + board.Size + " " + board.AdScore + " " + score);
        if (board.Size > 7 && (score - board.AdScore) >= 1000)
        {
            board.AdScore = score;
            board.interstitialAd.ShowInterstitialAd();
        }
    }
}