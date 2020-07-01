using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChestCanvas : MonoBehaviour {

    public GameObject buttonLayout;
    public Button itemPrefab;

    public TextMeshProUGUI currentItemView, foundItemView;

    private List<Button> createdButtons = new List<Button>();

    public GameObject itemSelectedObject;
    private PickableSO[] items;

    void OnEnable() {
        itemSelectedObject.SetActive(false);
    }

    public void ShowChest(PickableSO[] items) {
        // Close if already open
        if (gameObject.activeSelf) {
            CloseChest();
            return;
        }

        this.items = items;

        gameObject.SetActive(true);

        foreach (PickableSO item in items) {
            Button button = Instantiate(itemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            // Add button to the list and set the scale to 1 (parent.transform changes it to around 0,6)
            button.transform.SetParent(buttonLayout.transform);
            button.transform.localScale = Vector3.one;

            // Set name to the button
            button.GetComponentInChildren<TextMeshProUGUI>().text = item.name;
            createdButtons.Add(button);

            // Set button click listener
            button.onClick.AddListener(() => ItemSelected(item));
        }
    }

    public void CloseChest() {
        gameObject.SetActive(false);

        // Remove buttons
        foreach (Button btn in createdButtons) {
            Destroy(btn.gameObject);
        }
        createdButtons.Clear();
    }

    private void ItemSelected(PickableSO item) {
        itemSelectedObject.SetActive(true);
        foundItemView.text = item.name;

        // Set current item text
        if (item is WeaponSO)
            currentItemView.text = PlayerStats.Instance.player.GetWeapon().name;
        else if (item is ArmorSO)
            currentItemView.text = PlayerStats.Instance.player.GetArmor().name;
    }
}
