﻿#pragma warning disable 0649
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

    private Coroutine coroutineToHide, coroutineToDisable;
    private float timeToShow = 3;
    private float transitionSpd = 1;

    /************** 
     * METHODS FOR DIFFERENT TEXTS
     * 
     * I am using a system like this, since I don't have time to learn how to use xml string resources
     * in Unity. If I would know how to do that, then I would only need one method. Using a system like
     * this also collects all the important strings on the same class, so it's easy to make adjustments
     * to them and possibly use different languages.
     * **************/
    public void ShowGainStatText(Stat stat) {
        ShowTopInfoText($"You gained '{stat.name}' stat bonus!");
    }

    public void ShowStatsMaxedText(WordsType type) {
        ShowTopInfoText($"All stats for '{type.ToString()}' answers are maxed out!");
    }

    public void ShowShieldRecoveredText(float amount) {
        ShowTopInfoText($"You recovered shields by {amount.ToString()} %!");
    }

    public void ShowHealthRecoveredText() {
        ShowTopInfoText($"You recovered health!");
    }

    public void ShowBoostText(string boostType, float amount, float time) {
        ShowTopInfoText($"You activated {(amount * 100).ToString()} % {boostType} boost for {time.ToString()} seconds!");
    }

    public void ShowItemBrokeText(string itemName) {
        ShowTopInfoText($"Item {itemName} broke!");
    }

    public void ShowXpPercentageGainedText(float percentage) {
        ShowTopInfoText($"You gained {percentage.ToString()} % exp for next level!");
    }

    public void ShowScrapToToy(string scrapName, string toyName) {
        ShowTopInfoText($"{scrapName} successfully turned into a {toyName}!");
    }

    public void ShowIdentifiableEmpty() {
        ShowTopInfoText("You don't have any identifiable items!");
    }

    public void ShowBatteryIdentified(string batteryType) {
        ShowTopInfoText($"Battery is of '{batteryType}' type!");
    }

    /************** CLASS METHODS **************/

    public void Initialize() {
        // Hide panel
        animator = CanvasMaster.Instance.uiAnimator;
        ResetPosition();
    }

    private void ResetPosition() {
        infoObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, hideYPosition);
    }

    private void ShowTopInfoText(string textToShow) {
        ResetPosition();
        gameObject.SetActive(true);
        // Stop coroutine to hide the object if multiple texts are shown simultaneously
        if (coroutineToHide != null)
            StopCoroutine(coroutineToHide);

        // Stop coroutine to disable the object if info is currently closing
        if (coroutineToDisable != null)
            StopCoroutine(coroutineToDisable);

        textView.text = textToShow;

        animator.MoveY(infoObject, 0, transitionSpd, tweenType).
            setOnComplete(() => coroutineToHide = Helper.Instance.InvokeRealTime(() => HideTopInfo(), timeToShow));
    }

    private void HideTopInfo() {
        animator.MoveY(infoObject, hideYPosition, transitionSpd, tweenType);
        coroutineToDisable = Helper.Instance.InvokeRealTime(() => gameObject.SetActive(false), transitionSpd);
    }
}