using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler {

    private CanvasSounds sounds;
    private Button thisButton;
    private Navigation customNav = new Navigation();

    // Button specific
    public bool muteClickSound = false;
    public bool playBackSound = false;

    void Awake() {
        sounds = CanvasMaster.Instance.canvasSounds;
        thisButton = GetComponent<Button>();

        // Disable navigation mode for button
        customNav.mode = Navigation.Mode.None;
        thisButton.navigation = customNav;

        // Change button colors
        ColorBlock colors = thisButton.colors;
        colors.highlightedColor = new Color32(221, 221, 221, 255);
        thisButton.colors = colors;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (muteClickSound)
            return;

        PlaySound(playBackSound ? sounds.BUTTON_BACK : sounds.BUTTON_SELECT);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        PlaySound(sounds.BUTTON_ENTER);
    }

    private void PlaySound(AudioClip sound) {
        if (!thisButton.interactable)
            return;

        sounds.PlaySound(sound);
    }
}
