using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCanvas : MonoBehaviour {

    public GameObject weaponObj, armorObj, consumablesObj, miscObj;
    private GameObject[] categoryObjects;
    private float objCategoryStartX = -90;

    private enum MenuState { Closed, Categories, Weapon, Armor, Consumables, Miscellaneous }
    private MenuState curState = MenuState.Closed;

    public GameObject weaponStatsObject;

    public LeanTweenType tweenType;
    private float tweenTime = 0.5f;

    void Awake() {
        categoryObjects = new GameObject[] { weaponObj, armorObj, consumablesObj, miscObj };

        ResetElementPositions();
    }

    public void ToggleMenu() {
        foreach (GameObject obj in categoryObjects) {
            if (curState == MenuState.Closed) {
                // Open menu
                LeanTween.scale(obj, Vector3.one, tweenTime).
                setEase(tweenType);
                LeanTween.moveLocalX(obj, 0, tweenTime).setEase(tweenType);
            } else {
                // Close menu
                LeanTween.scale(obj, Vector3.zero, tweenTime).
                setEase(tweenType);
                LeanTween.moveLocalX(obj, objCategoryStartX, tweenTime).setEase(tweenType).
                    setOnComplete(ResetElementPositions);
            }
        }

        // Toggle menu state
        if (curState == MenuState.Closed)
            curState = MenuState.Categories;
        else
            curState = MenuState.Closed;
    }

    public void ToggleShowWeapon() {
        WeaponStatHolder holder = weaponStatsObject.GetComponent<WeaponStatHolder>();
        WeaponSO weapon = Inventory.Instance.equippedWeapon;
        Helper.Instance.SetupWeaponStats(holder, weapon);

        if (curState != MenuState.Weapon) {
            curState = MenuState.Weapon;
            weaponStatsObject.SetActive(true);
        } else {
            curState = MenuState.Categories;
            weaponStatsObject.SetActive(false);
        }
    }

    private void ResetElementPositions() {
        // Hide categories
        foreach (GameObject obj in categoryObjects) {
            obj.transform.LeanMoveLocalX(objCategoryStartX, 0f);
            obj.transform.localScale = Vector3.zero;
        }

        // Hide row 2 stuff
        weaponStatsObject.SetActive(false);
    }
}
