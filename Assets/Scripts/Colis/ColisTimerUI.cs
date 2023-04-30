using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColisTimerUI : MonoBehaviour
{
    [SerializeField] private Image timerImage;

    [SerializeField] private float maxGreenColorTimeNormalized = .25f;
    [SerializeField] private float maxYellowColorTimeNormalized = .5f;
    [SerializeField] private float maxRedColorTimeNormalized = .75f;

    [SerializeField] private float blinkspeed = 2;

    private float currentBlinkValueNormalized = 0;

    public void UpdateTimer(float _normalizedTimer)
    {
        timerImage.fillAmount = _normalizedTimer;

        if (_normalizedTimer < maxGreenColorTimeNormalized)
            timerImage.color = Color.green;
        else if (_normalizedTimer < maxYellowColorTimeNormalized)
        {
            float _split = maxYellowColorTimeNormalized - maxGreenColorTimeNormalized;
            float _multiplicator = 1 / _split;
            float _tValue = (_normalizedTimer - maxGreenColorTimeNormalized) * _multiplicator;

            timerImage.color = new Color(Mathf.Lerp(0, 1, _tValue), 1, 0, 1);
        }
        else if (_normalizedTimer < maxRedColorTimeNormalized)
        {
            float _split = maxRedColorTimeNormalized - maxYellowColorTimeNormalized;
            float _multiplicator = 1 / _split;
            float _tValue = (_normalizedTimer - maxYellowColorTimeNormalized) * _multiplicator;

            timerImage.color = new Color(1, Mathf.Lerp(1, 0, _tValue), 0, 1);
        }
        else
            BlinkRed();
    }

    private void BlinkRed()
    {
        currentBlinkValueNormalized = Mathf.PingPong(Time.time * blinkspeed, 1);

        timerImage.color = new Color(1, currentBlinkValueNormalized, currentBlinkValueNormalized, 1);
    }
}
