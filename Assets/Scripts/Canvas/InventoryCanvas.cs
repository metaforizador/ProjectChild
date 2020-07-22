using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryCanvas : MonoBehaviour {

    public GameObject menuAndCategories, categoriesParent, openedCategoryParent;
    public GameObject weaponObj, armorObj, consumablesObj, miscObj;
    private GameObject[] categoryObjects;
    private float objCategoryStartX = -90;
    private Vector3 scaledMenuAndCategories = new Vector3(0.7f, 0.7f, 0.7f);

    private UIAnimator animator;

    // DELETE LATER
    public GameObject openDebugMenuButton;
    public GameObject debugMenu;

    public void ToggleDebugMenu() {
        debugMenu.SetActive(!debugMenu.activeSelf);
    }

    private bool menuOpen = false;
    private int currentlyOpen;

    private const int NONE = 0, WEAPON = 1, ARMOR = 2, CONSUMABLES = 3, MISC = 4;

    // Categories
    public GameObject weaponStatsObject, armorStatsObject, consumablesObject;

    // ConsumableItems
    public ConsumablesScrollSystem consumableScrollSystem;
    public TextMeshProUGUI selectedItemName, selectedItemDescription;
    public GameObject itemStatsDisplay, scannerStats, batteryStats, comsatLinkStats, rigStats, scrapStats, toyStats;
    private ConsumableSO selectedItem;
    private HotbarCanvas hotbar;

    private CanvasSounds sounds;

    public LeanTweenType tweenType;
    private float tweenTime = 0.35f;

    void Awake() {
        CanvasMaster cv = CanvasMaster.Instance;
        animator = cv.uiAnimator;
        sounds = cv.canvasSounds;
        hotbar = cv.hotbarCanvas.GetComponent<HotbarCanvas>();
        categoryObjects = new GameObject[] { weaponObj, armorObj, consumablesObj, miscObj };

        // Hide debug menu stuff
        debugMenu.SetActive(false);
        openDebugMenuButton.SetActive(false);

        // Hide categories
        categoriesParent.SetActive(false);
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
        // Loop through all categories and animate them
        foreach (GameObject obj in categoryObjects) {
            if (!menuOpen) {
                // Open menu
                animator.MoveX(obj, 0, tweenTime, tweenType);
                animator.Scale(obj, Vector3.one, tweenTime, tweenType);
            } else {
                // Close menu
                animator.Scale(obj, Vector3.zero, tweenTime, tweenType);
                animator.MoveX(obj, objCategoryStartX, tweenTime, tweenType);
            }
        }

        // Toggle menu state
        menuOpen = !menuOpen;

        GameMaster gm = GameMaster.Instance;

        if (menuOpen) {
            // Play sound when opening the menu
            sounds.PlaySound(sounds.BUTTON_SELECT);
            openDebugMenuButton.SetActive(true);
            categoriesParent.SetActive(true);
            gm.SetState(GameState.Menu);
        } else {
            // Play sound when closing the menu and hide opened categories
            sounds.PlaySound(sounds.BUTTON_BACK);
            ShowRequiredCategory(NONE);
            // Change state and hide debug stuff
            Helper.Instance.InvokeRealTime(() => {
                categoriesParent.SetActive(false);
                gm.SetState(GameState.Movement);
                openDebugMenuButton.SetActive(false);
                debugMenu.SetActive(false);
            }, tweenTime);
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
        consumableScrollSystem.ClearAllItems();

        // Get consumables from inventory
        List<ConsumableSO> consumables = Inventory.Instance.GetConsumables();
        foreach (ConsumableSO con in consumables) {
            consumableScrollSystem.AddItem(con).
                // Show item information when clicked
                onClick.AddListener(() => ShowItemInfo(con));
        }

        ShowRequiredCategory(CONSUMABLES);
        itemStatsDisplay.SetActive(false); // Disable item stats display since item is not yet chosen
    }

    public void CloseSubcategory() {
        ShowRequiredCategory(NONE);
        sounds.PlaySound(sounds.BUTTON_BACK);
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
                holder.boostStaminaRecoverySpeed.text = (con.boostStaminaRecoverySpeed * 100).ToString() + "%";
                holder.boostAmmoRecoverySpeed.text = (con.boostAmmoRecoverySpeed * 100).ToString() + "%";
                holder.boostTimeInSeconds.text = con.boostTimeInSeconds.ToString();
                holder.batteryType.text = con.batteryType.ToString();
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
                holder.toyWordsType.text = con.toyWordsType.ToString();
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

    public void ShowHotbarItemInfo(ConsumableSO consumable) {
        if (currentlyOpen != CONSUMABLES) {
            ToggleConsumables();
        }

        ShowItemInfo(consumable);
    }

    public void EquipItem() {
        sounds.PlaySound(sounds.BUTTON_SELECT);
        hotbar.SetIncomingItem(selectedItem);
    }

    public void UseItem() {
        Inventory.Instance.UseConsumable(selectedItem);
    }

    /// <summary>
    /// Refresh consumables when they are used.
    /// 
    /// This method is called from Inventory singleton.
    /// </summary>
    public void RefreshConsumables() {
        // Refresh only if menu is open
        if (GameMaster.Instance.gameState.Equals(GameState.Menu)) {
            ShowRequiredCategory(NONE);
            ToggleConsumables();
            Debug.Log(selectedItem.quantity);
            if (selectedItem.quantity > 0)
                ShowItemInfo(selectedItem);
            else
                selectedItem = null;
        }
    }

    /// <summary>
    /// Scales menu and categories smaller or to default size.
    /// </summary>
    /// <param name="smaller">whether to scale smaller or not</param>
    private void ScaleMenuAndCategories(bool smaller) {
        if (smaller)
            animator.Scale(menuAndCategories, scaledMenuAndCategories, tweenTime, tweenType);
        else
            animator.Scale(menuAndCategories, Vector3.one, tweenTime, tweenType);
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
        openedCategoryParent.SetActive(currentlyOpen != NONE);
        weaponStatsObject.SetActive(currentlyOpen == WEAPON);
        armorStatsObject.SetActive(currentlyOpen == ARMOR);
        consumablesObject.SetActive(currentlyOpen == CONSUMABLES);
    }
}
