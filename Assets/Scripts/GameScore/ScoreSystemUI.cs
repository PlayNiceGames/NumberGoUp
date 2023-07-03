using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Utils;

public class ScoreSystemUI : MonoBehaviour
{
    [SerializeField] private float _textUpdateTimeSeconds;
    [SerializeField] private AnimationCurve _textUpdateCurve;

    [SerializeField] private TextMeshProUGUI _label;

    public void SetScore(int score)
    {
        UpdateText(score);
    }

    public UniTask UpdateScoreWithAnimation(int score, int prevScore)
    {
        var tween = DOTween.To(() => prevScore, UpdateText, score, _textUpdateTimeSeconds);
        tween.SetEase(_textUpdateCurve);

        return tween.PlayAsync();
    }

    private void UpdateText(int score)
    {
        _label.text = score.ToString();
    }
}