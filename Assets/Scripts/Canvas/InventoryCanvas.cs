using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryCanvas : MonoBehaviour {

    public GameObject menuAndCategories;
    public GameObject weaponObj, armorObj, consumablesObj, miscObj;
    private GameObject[] categoryObjects;
    private float objCategoryStartX = -90;
    private Vector3 scaledMenuAndCategories = new Vector3(0.7f, 0.7f, 0.7f);

    private bool menuOpen = false;
    private int currentlyOpen;

    private const int NONE = 0, WEAPON = 1, ARMOR = 2, CONSUMABLES = 3, MISC = 4;

    // Categories
    public GameObject weaponStatsObject, armorStatsObject, consumablesObject;

    // ConsumableItems
    public GameObject consumableItemPrefab, consumableContent;
    public TextMeshProUGUI selectedItemName, selectedItemDescription;
    public GameObject itemStatsDisplay, scannerStats, batteryStats, comsatLinkStats, rigStats, scrapStats, toyStats;
    private ConsumableSO selectedItem;
    private HotbarCanvas hotbar;

    private CanvasSounds sounds;

    public LeanTweenType tweenType;
    private float tweenTime = 0.35f;

    void Awake() {
        sounds = CanvasMaster.Instance.canvasSounds;
        hotbar = CanvasMaster.Instance.hotbarCanvas.GetComponent<HotbarCanvas>();
        categoryObjects = new GameObject[] { weaponObj, armorObj, consumablesObj, miscObj };

        // Hide categories
        foreach (GameObject obj in categoryObjects) {
            obj.transform.LeanMoveLocalX(objCategoryStartX, 0f);
            obj.transform.localScale = Vector3.zero;
        }

        ShowRequiredCategory(NONE);
    }

    /// <summary>
    /// Shows and hides all of the categories.
    /// </summary>
    public void ToggleMenu() {
        foreach (GameObject obj in categoryObjects) {
            if (!menuOpen) {
                // Open menu
                LeanTween.scale(obj, Vector3.one, tweenTime).setEase(tweenType);
                LeanTween.moveLocalX(obj, 0, tweenTime).setEase(tweenType);
            } else {
                // Close menu
                LeanTween.scale(obj, Vector3.zero, tweenTime).setEase(tweenType);
                LeanTween.moveLocalX(obj, objCategoryStartX, tweenTime).setEase(tweenType);
            }
        }

        // Toggle menu state
        menuOpen = !menuOpen;

        // Show / hide cursor
        GameMaster.Instance.ShowCursor(menuOpen);

        if (menuOpen) {
            // Play sound when opening the menu
            sounds.PlaySound(sounds.BUTTON_SELECT);
        } else {
            // Play sound when closing the menu and hide opened categories
            sounds.PlaySound(sounds.BUTTON_BACK);
            ShowRequiredCategory(NONE);
        }
    }

    /// <summary>
    /// Shows and hides weapon stats information.
    /// </summary>
    public void ToggleWeapon() {
        ShowRequiredCategory(WEAPON);
        
        WeaponStatHolder holder = weaponStatsObject.GetComponent<WeaponStatHolder>();
        WeaponSO weapon = Inventory.Instance.equippedWeapon;
        Helper.Instance.SetupWeaponStats(holder, weapon);
    }

    /// <summary>
    /// Shows and hides armor stats information.
    /// </summary>
    public void ToggleArmor() {
        ShowRequiredCategory(ARMOR);

        ArmorStatHolder holder = armorStatsObject.GetComponent<ArmorStatHolder>();
        ArmorSO armor = Inventory.Instance.equippedArmor;
        Helper.Instance.SetupArmorStats(holder, armor);
    }

    /// <summary>
    /// Shows and hides consumables in inventory.
    /// </summary>
    public void ToggleConsumables() {
        // Destroy all previous consumable item buttons
        foreach (Transform child in consumableContent.transform) {
            Destroy(child.gameObject);
        }

        // Get consumables from inventory
        List<ConsumableSO> consumables = Inventory.Instance.GetConsumables();
        foreach (ConsumableSO con in consumables) {
            // Create button for each consumable
            GameObject btn = Helper.Instance.CreateObjectChild(consumableItemPrefab, consumableContent);
            // Add name to the consumable
            btn.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = con.name;
            // Show item information when clicked
            btn.GetComponent<Button>().onClick.AddListener(() => ShowItemInfo(con));
        }

        ShowRequiredCategory(CONSUMABLES);
        itemStatsDisplay.SetActive(false); // Disable item stats display since item is not yet chosen
    }

    /// <summary>
    /// Shows item's information.
    /// </summary>
    /// <param name="con">item to get the information from</param>
    private void ShowItemInfo(ConsumableSO con) {
        sounds.PlaySound(sounds.BUTTON_SELECT);
        ConsumableStatHolder holder;
        ConsumableType type = con.consumableType;
        // Activate correct objects
        itemStatsDisplay.SetActive(true);
        ShowCorrectItemStats(type);
        // Set selected item for equip and use buttons
        selectedItem = con;
        // Set name
        selectedItemName.text = con.name;

        // Set description text and other stat texts based on consumable type
        switch (type) {
            case ConsumableType.Scanner:
                selectedItemDescription.text = ConsumableSO.DESCRIPTION_SCANNER;
                holder = scannerStats.GetComponent<ConsumableStatHolder>();
                holder.identificationChance.text = con.identificationChance.ToString() + "%";
                break;
            case ConsumableType.Battery:
                selectedItemDescription.text = ConsumableSO.DESCRIPTION_BATTERY;
                holder = batteryStats.GetComponent<ConsumableStatHolder>();
                holder.shieldRecoveryPercentage.text = con.shieldRecoveryPercentage.ToString() + "%";
                break;
            case ConsumableType.ComsatLink:
                selectedItemDescription.text = ConsumableSO.DESCRIPTION_COMSAT_LINK;
                holder = comsatLinkStats.GetComponent<ConsumableStatHolder>();
                holder.chanceToBeSuccessful.text = con.chanceToBeSuccessful.ToString() + "%";
                break;
            case ConsumableType.Rig:
                selectedItemDescription.text = ConsumableSO.DESCRIPTION_RIG;
                holder = rigStats.GetComponent<ConsumableStatHolder>();
                holder.chanceToBeSuccessful.text = con.chanceToBeSuccessful.ToString() + "%";
                break;
            case ConsumableType.Scrap:
                selectedItemDescription.text = ConsumableSO.DESCRIPTION_SCRAP;
                holder = scrapStats.GetComponent<ConsumableStatHolder>();
                holder.creditValue.text = con.creditValue.ToString();
                holder.craftValue.text = con.craftValue.ToString();
                holder.chanceToTurnIntoToy.text = con.chanceToTurnIntoToy.ToString() + "%";
                break;
            case ConsumableType.Toy:
                selectedItemDescription.text = ConsumableSO.DESCRIPTION_TOY;
                holder = toyStats.GetComponent<ConsumableStatHolder>();
                holder.expToGain.text = con.expToGain.ToString() + "%";
                break;
        }
    }

    /// <summary>
    /// Shows and hides item stats which should be shown and hidden.
    /// </summary>
    /// <param name="type">type of stats to show</param>
    private void ShowCorrectItemStats(ConsumableType type) {
        scannerStats.SetActive(type.Equals(ConsumableType.Scanner));
        batteryStats.SetActive(type.Equals(ConsumableType.Battery));
        comsatLinkStats.SetActive(type.Equals(ConsumableType.ComsatLink));
        rigStats.SetActive(type.Equals(ConsumableType.Rig));
        scrapStats.SetActive(type.Equals(ConsumableType.Scrap));
        toyStats.SetActive(type.Equals(ConsumableType.Toy));
    }

    public void EquipItem() {
        //hotbar.SetIncomingItem(selectedItem);
    }

    public void UseItem() {

    }

    /// <summary>
    /// Scales menu and categories smaller or to default size.
    /// </summary>
    /// <param name="smaller">whether to scale smaller or not</param>
    private void ScaleMenuAndCategories(bool smaller) {
        if (smaller)
            LeanTween.scale(menuAndCategories, scaledMenuAndCategories, tweenTime).setEase(tweenType);
        else
            LeanTween.scale(menuAndCategories, Vector3.one, tweenTime).setEase(tweenType);
    }

    /// <summary>
    /// Shows and hides correct categories.
    /// </summary>
    /// <param name="category">category which should be open</param>
    private void ShowRequiredCategory(int category) {
        // If category was open and player clicked the same category, close it
        if (category == currentlyOpen && category != NONE) {
            sounds.PlaySound(sounds.BUTTON_BACK);
            category = NONE;
        }
        
        // If element is none, reset menu and categories scale
        if (category == NONE) {
            ScaleMenuAndCategories(false);
        } else {
            // Else category is opened, so play sound
            sounds.PlaySound(sounds.BUTTON_SELECT);
            ScaleMenuAndCategories(true);
        }

        currentlyOpen = category;

        // Set objects active based on if they should be currently open
        weaponStatsObject.SetActive(currentlyOpen == WEAPON);
        armorStatsObject.SetActive(currentlyOpen == ARMOR);
        consumablesObject.SetActive(currentlyOpen == CONSUMABLES);
    }
}
