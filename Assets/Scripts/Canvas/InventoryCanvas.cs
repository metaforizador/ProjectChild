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

    public GameObject weaponStatsObject, armorStatsObject;

    public LeanTweenType tweenType;
    private float tweenTime = 0.35f;

    void Awake() {
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
                ShowRequiredElements(NONE);
                LeanTween.scale(obj, Vector3.zero, tweenTime).
                setEase(tweenType);
                LeanTween.moveLocalX(obj, objCategoryStartX, tweenTime).setEase(tweenType);
            }
        }

        // Toggle menu state
        menuOpen = !menuOpen;
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
        // If element is none or the one currently open, hide open category
        if (element == NONE || currentlyOpen == element) {
            ScaleMenuAndCategories(false);
            currentlyOpen = NONE;
        } else {
            currentlyOpen = element;
            ScaleMenuAndCategories(true);
        }

        // Set objects active based on if they should be currently open
        weaponStatsObject.SetActive(currentlyOpen == WEAPON);
        armorStatsObject.SetActive(currentlyOpen == ARMOR);
    }
}
