using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChestCanvas : MonoBehaviour {

    public GameObject buttonLayout;
    public Button itemPrefab;

    private Chest openedChest;

    public TextMeshProUGUI currentItemView, foundItemView;

    private List<Button> createdItemButtons = new List<Button>();

    public GameObject itemSelectedObject;
    private PickableSO[] items;
    private int selectedItemIndex;

    // Item stats
    public GameObject weaponStatsPrefab, armorStatsPrefabLeft, armorStatsPrefabRight;
    public GameObject currentItemStats, selectedItemStats;

    void OnEnable() {
        itemSelectedObject.SetActive(false);
    }

    public void ShowChest(Chest chest, PickableSO[] items) {
        // Close if already open
        if (gameObject.activeSelf) {
            CloseChest();
            return;
        }

        this.openedChest = chest;
        this.items = items;

        gameObject.SetActive(true);

        for (int i = 0; i < items.Length; i++) {
            PickableSO item = items[i];

            Button button = Instantiate(itemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            // Add button to the list and set the scale to 1 (parent.transform changes it to around 0,6)
            button.transform.SetParent(buttonLayout.transform);
            button.transform.localScale = Vector3.one;

            // Set name to the button
            button.GetComponentInChildren<TextMeshProUGUI>().text = item.name;
            createdItemButtons.Add(button);

            // Set button click listener
            int index = i;  // Local variable needed because of for loop
            button.onClick.AddListener(() => ItemSelected(index));
        }
    }

    /// <summary>
    /// Called when item is selected.
    /// 
    /// Opens item select elements and changes texts.
    /// </summary>
    /// <param name="index"></param>
    private void ItemSelected(int index) {
        selectedItemIndex = index;
        PickableSO item = items[index];

        itemSelectedObject.SetActive(true); // Activate item select elements
        foundItemView.text = item.name;     // Change found item text

        // Set item texts and stats
        if (item is WeaponSO) {
            currentItemView.text = PlayerStats.Instance.player.GetWeapon().name;
            ShowWeaponStats();
        } else if (item is ArmorSO) {
            currentItemView.text = PlayerStats.Instance.player.GetArmor().name;
            ShowArmorStats();
        }
    }

    private void ShowWeaponStats() {
        // Setup current weapon stats
        WeaponStatHolder holder = Helper.Instance.CreateObjectChild(weaponStatsPrefab, currentItemStats).
            GetComponent<WeaponStatHolder>();
        WeaponSO weapon = PlayerStats.Instance.player.GetWeapon();
        SetupWeaponStats(holder, weapon);

        // Setup found weapon stats
        holder = Helper.Instance.CreateObjectChild(weaponStatsPrefab, selectedItemStats).
            GetComponent<WeaponStatHolder>();
        weapon = (WeaponSO) items[selectedItemIndex];
        SetupWeaponStats(holder, weapon);
    }

    private void SetupWeaponStats(WeaponStatHolder holder, WeaponSO weapon) {
        holder.type.text = weapon.weaponType.ToString();
        holder.damage.text = weapon.damagePerBullet.ToString();
        holder.bulletSpeed.text = weapon.bulletSpeed.ToString();
        holder.ammoSize.text = weapon.ammoSize.ToString();
        holder.rateOfFire.text = (60 / weapon.rateOfFire).ToString();   // Rounds per minute
    }

    private void ShowArmorStats() {
        // Setup current armor stats
        ArmorStatHolder holder = Helper.Instance.CreateObjectChild(armorStatsPrefabLeft, currentItemStats).
            GetComponent<ArmorStatHolder>();
        ArmorSO armor = PlayerStats.Instance.player.GetArmor();
        SetupArmorStats(holder, armor);

        // Setup found armor stats
        holder = Helper.Instance.CreateObjectChild(armorStatsPrefabRight, selectedItemStats).
            GetComponent<ArmorStatHolder>();
        armor = (ArmorSO)items[selectedItemIndex];
        SetupArmorStats(holder, armor);
    }

    private void SetupArmorStats(ArmorStatHolder holder, ArmorSO armor) {
        holder.decreaseShieldDelay.text = armor.decreaseShieldRecoveryDelay.ToString();
        holder.increaseShield.text = armor.increaseShield.ToString();
        holder.lowerOpponentsCritChance.text = armor.decreaseOpponentCriticalRate.ToString();
        holder.lowerOpponentsCritMultiplier.text = armor.decreaseOpponentCriticalMultiplier.ToString();
        holder.decreaseMovementSpeed.text = armor.reduceMovementSpeed.ToString();
        holder.decreaseStaminaRecoveryRate.text = armor.reduceStaminaRecoveryRate.ToString();
    }

    /// <summary>
    /// Swaps player's weapon or armor.
    /// </summary>
    public void SwapItem() {
        Player player = PlayerStats.Instance.player;

        PickableSO selectedItem = items[selectedItemIndex];
        PickableSO oldItem = null;

        // Check the type of PickableSO
        if (selectedItem is WeaponSO) {
            oldItem = player.ChangeWeapon((WeaponSO)selectedItem);
        } else if (selectedItem is ArmorSO) {
            oldItem = player.ChangeArmor((ArmorSO)selectedItem);
        }

        // Replace selected item with the player's old item
        items[selectedItemIndex] = oldItem;

        // Change item contents inside the chest
        openedChest.ChangeItems(items);

        // Refresh chest contents
        CloseChest();
        ShowChest(openedChest, items);
    }

    public void Cancel() {
        itemSelectedObject.SetActive(false);
    }

    /// <summary>
    /// Closes the chest view.
    /// </summary>
    public void CloseChest() {
        gameObject.SetActive(false);

        // Remove buttons
        foreach (Button btn in createdItemButtons) {
            Destroy(btn.gameObject);
        }
        createdItemButtons.Clear();
    }
}
