using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatGainCanvas : MonoBehaviour {

    [SerializeField]
    private float hideYPosition;

    [SerializeField]
    private GameObject statObject;

    [SerializeField]
    private TextMeshProUGUI textView;

    public LeanTweenType tweenType;

    private float timeToShow = 3;
    private float transitionSpd = 1;

    public static string CreateGainStatText(Stat stat) {
        return $"You gained '{stat.name}' stat bonus!";
    }

    public static string CreateStatsMaxedText(WordsType type) {
        return $"All stats for '{type.ToString()}' answers are maxed out!";
    }

    void OnEnable() {
        // Hide panel
        statObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, hideYPosition);
    }

    void onStart() {
        statObject.transform.LeanMoveY(hideYPosition, 0f);
    }

    public void ShowStatGain(string statGainText) {
        textView.text = statGainText;
        LeanTween.moveLocalY(statObject, 0, transitionSpd).
            setEase(tweenType).
            setOnComplete(() => Invoke("HideStatGain", timeToShow));
    }

    private void HideStatGain() {
        LeanTween.moveLocalY(statObject, hideYPosition, transitionSpd).setEase(tweenType);
    }
}
