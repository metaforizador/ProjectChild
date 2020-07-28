using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(Button))]
public class ScrollbarIncrementer : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {
    public Scrollbar target;
    private float step = 0.05f; // Amount of steps one click moves the rect

    private float holdFrequency = 0.05f; // Seconds to wait when holding
    public bool increment;

    void Start() {
        // Enable / disable button based on scrollbar value
        target.onValueChanged.AddListener((value) => {
            GetComponent<Button>().interactable = increment ? target.value < 1 : target.value > 0;
        });
    }

    /// <summary>
    /// Increment and decrement the scroll wheel value.
    /// </summary>
    public void MoveScroll() {
        if (target == null) throw new Exception("Setup ScrollbarIncrementer first!");
        float value = increment ? target.value + step : target.value - step;
        target.value = Mathf.Clamp(value, 0, 1);
    }

    IEnumerator IncrementDecrementSequence(bool increment) {
        yield return new WaitForSecondsRealtime(holdFrequency);
        MoveScroll();
        StartCoroutine("IncrementDecrementSequence", increment);
    }

    public void OnPointerClick(PointerEventData eventData) {
        MoveScroll();
    }

    public void OnPointerDown(PointerEventData eventData) {
        StartCoroutine("IncrementDecrementSequence", increment);
    }

    public void OnPointerUp(PointerEventData eventData) {
        StopCoroutine("IncrementDecrementSequence");
    }
}