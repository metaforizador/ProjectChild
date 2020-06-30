using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChestCanvas : MonoBehaviour {

    public GameObject buttonLayout;
    public Button itemPrefab;

    private List<Button> createdButtons = new List<Button>();

    public void ShowChest(PickableSO[] items) {
        // Close if already open
        if (gameObject.activeSelf) {
            CloseChest();
            return;
        }

        gameObject.SetActive(true);

        foreach (PickableSO item in items) {
            Button button = Instantiate(itemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            // Add button to the list and set the scale to 1 (parent.transform changes it to around 0,6)
            button.transform.SetParent(buttonLayout.transform);
            button.transform.localScale = Vector3.one;

            // Set name to the button
            button.GetComponentInChildren<TextMeshProUGUI>().text = item.name;
            createdButtons.Add(button);
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
}
