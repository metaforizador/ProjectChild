#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TopInfoCanvas : MonoBehaviour {

    [SerializeField]
    private float hideYPosition;

    [SerializeField]
    private GameObject infoObject;

    [SerializeField]
    private TextMeshProUGUI textView;

    private UIAnimator animator;
    public LeanTweenType tweenType;

    private float timeToShow = 3;
    private float transitionSpd = 1;

    public static string CreateGainStatText(Stat stat) {
        return $"You gained '{stat.name}' stat bonus!";
    }

    public static string CreateStatsMaxedText(WordsType type) {
        return $"All stats for '{type.ToString()}' answers are maxed out!";
    }

    void Awake() {
        animator = CanvasMaster.Instance.uiAnimator;
    }

    public void Initialize() {
        // Hide panel
        infoObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, hideYPosition);
    }

    public void ShowTopInfoText(string textToShow) {
        textView.text = textToShow;
        animator.MoveY(infoObject, 0, transitionSpd, tweenType).
            setOnComplete(() => Helper.Instance.InvokeRealTime(() => HideTopInfo(), timeToShow));
    }

    private void HideTopInfo() {
        animator.MoveY(infoObject, hideYPosition, transitionSpd, tweenType).
            setOnComplete(() => gameObject.SetActive(false));
    }
}
