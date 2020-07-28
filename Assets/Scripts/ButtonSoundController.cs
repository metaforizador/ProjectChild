using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSoundController : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler {

    CanvasSounds sounds;

    void Awake() {
        sounds = CanvasMaster.Instance.canvasSounds;
    }

    public void OnPointerClick(PointerEventData eventData) {
        sounds.PlaySound(sounds.BUTTON_SELECT);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        sounds.PlaySound(sounds.BUTTON_ENTER);
    }
}
