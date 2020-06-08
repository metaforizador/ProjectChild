using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatGainCanvas : MonoBehaviour {

    [SerializeField]
    private float hideYPosition, showYPosition;

    [SerializeField]
    private GameObject statObject;

    [SerializeField]
    private TextMeshProUGUI textView;

    public LeanTweenType tweenType;

    private string gainTextStart = "You increased your ";
    private string gainTextEnd = "stats!";
    private float timeToShow = 5;
    private float transitionSpd = 1;

    void OnEnable() {
        // Hide panel
        statObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, hideYPosition);
    }

    void onStart() {
        statObject.transform.LeanMoveY(hideYPosition, 0f);
    }

    public void ShowStatGain(string gainedStats) {
        textView.text = gainTextStart + gainedStats + gainTextEnd;
        LeanTween.moveLocalY(statObject, showYPosition, transitionSpd).
            setEase(tweenType).
            setOnComplete(() => Invoke("HideStatGain", timeToShow));
    }

    private void HideStatGain() {
        LeanTween.moveLocalY(statObject, hideYPosition, transitionSpd).setEase(tweenType);
    }
}
