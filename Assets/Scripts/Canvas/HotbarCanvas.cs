using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarCanvas : MonoBehaviour {

    public GameObject buttonLayout;
    public Sprite defaultImage;
    public Color defaultColor;

    private GameObject[] hotbarButtons;
    private ConsumableSO[] hotbarItems;

    public GameObject changingItemsPanel;
    private ConsumableSO incomingItem;

    void Awake() {
        changingItemsPanel.SetActive(false);
    }

    void Start() {
        int buttonAmount = buttonLayout.transform.childCount;
        hotbarButtons = new GameObject[buttonAmount];
        hotbarItems = new ConsumableSO[buttonAmount];

        for (int i = 0; i < buttonAmount; i++) {
            GameObject btn = buttonLayout.transform.GetChild(i).gameObject;
            hotbarButtons[i] = btn;

            // Set onClickListener for button
            int index = i;
            btn.GetComponent<Button>().onClick.AddListener(() => HotbarButtonClicked(index));
        }

        RefreshHotbarImages();
    }

    private void HotbarButtonClicked(int index) {
        if (incomingItem != null) {
            SwapHotbarItem(index);
        } else {
            // Maybe show item info later?
        }
    }

    private void SwapHotbarItem(int index) {
        hotbarItems[index] = incomingItem;
        RefreshHotbarImages();
    }

    private void RefreshHotbarImages() {
        for (int i = 0; i < hotbarItems.Length; i++) {
            Image img = hotbarButtons[i].GetComponent<Image>();
            ConsumableSO consumable = hotbarItems[i];

            if (consumable != null) {
                img.sprite = consumable.sprite;
                img.color = Color.white;
            } else {
                img.sprite = defaultImage;
                img.color = defaultColor;
            }

            // Type changes to Simple for some reason when changing sprites
            // If type stays Simple, the default image changes shape, which we don't want
            img.type = Image.Type.Sliced;
        }
    }

    public void SetIncomingItem(ConsumableSO consumable) {
        changingItemsPanel.SetActive(true);
        incomingItem = consumable;
    }

    public void DisableIncomingItem() {
        changingItemsPanel.SetActive(false);
        incomingItem = null;
    }

    public void LoadHotbar(Save save) {
        hotbarItems = save.hotbarItems;

        RefreshHotbarImages();
    }

    public void SaveHotbar(Save save) {
        save.hotbarItems = hotbarItems;
    }
}
