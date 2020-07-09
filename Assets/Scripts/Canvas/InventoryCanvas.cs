using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCanvas : MonoBehaviour {

    public GameObject menuAndCategories;
    public GameObject weaponObj, armorObj, consumablesObj, miscObj;
    private GameObject[] categoryObjects;
    private float objCategoryStartX = -90;
    private Vector3 scaledMenuAndCategories = new Vector3(0.7f, 0.7f, 0.7f);

    private bool menuOpen = false;
    private int currentlyOpen;

    private const int NONE = 0, WEAPON = 1, ARMOR = 2, CONSUMABLES = 3, MISC = 4;

    public GameObject weaponStatsObject, armorStatsObject, consumablesObject;
    public GameObject consumableItemPrefab, consumableContent;

    private CanvasSounds sounds;

    public LeanTweenType tweenType;
    private float tweenTime = 0.35f;

    void Awake() {
        sounds = CanvasMaster.Instance.canvasSounds;
        categoryObjects = new GameObject[] { weaponObj, armorObj, consumablesObj, miscObj };

        // Hide categories
        foreach (GameObject obj in categoryObjects) {
            obj.transform.LeanMoveLocalX(objCategoryStartX, 0f);
            obj.transform.localScale = Vector3.zero;
        }

        ShowRequiredElements(NONE);
    }

    public void ToggleMenu() {
        foreach (GameObject obj in categoryObjects) {
            if (!menuOpen) {
                // Open menu
                LeanTween.scale(obj, Vector3.one, tweenTime).
                setEase(tweenType);
                LeanTween.moveLocalX(obj, 0, tweenTime).setEase(tweenType);
            } else {
                // Close menu
                LeanTween.scale(obj, Vector3.zero, tweenTime).
                setEase(tweenType);
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
            ShowRequiredElements(NONE);
        }
    }

    public void ToggleWeapon() {
        ShowRequiredElements(WEAPON);
        
        WeaponStatHolder holder = weaponStatsObject.GetComponent<WeaponStatHolder>();
        WeaponSO weapon = Inventory.Instance.equippedWeapon;
        Helper.Instance.SetupWeaponStats(holder, weapon);
    }

    public void ToggleArmor() {
        ShowRequiredElements(ARMOR);

        ArmorStatHolder holder = armorStatsObject.GetComponent<ArmorStatHolder>();
        ArmorSO armor = Inventory.Instance.equippedArmor;
        Helper.Instance.SetupArmorStats(holder, armor);
    }

    public void ToggleConsumables() {
        foreach (Transform child in consumableContent.transform) {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < 20; i++) {
            Helper.Instance.CreateObjectChild(consumableItemPrefab, consumableContent);
        }

        ShowRequiredElements(CONSUMABLES);
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

    private void ShowRequiredElements(int element) {
        // If category was open and player clicked the same category, close it
        if (element == currentlyOpen && element != NONE) {
            sounds.PlaySound(sounds.BUTTON_BACK);
            element = NONE;
        }
        
        // If element is none, reset menu and categories scale
        if (element == NONE) {
            ScaleMenuAndCategories(false);
        } else {
            // Else category is opened, so play sound
            sounds.PlaySound(sounds.BUTTON_SELECT);
            ScaleMenuAndCategories(true);
        }

        currentlyOpen = element;

        // Set objects active based on if they should be currently open
        weaponStatsObject.SetActive(currentlyOpen == WEAPON);
        armorStatsObject.SetActive(currentlyOpen == ARMOR);
        consumablesObject.SetActive(currentlyOpen == CONSUMABLES);
    }
}
