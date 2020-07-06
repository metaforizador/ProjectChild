using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCanvas : MonoBehaviour {

    public GameObject weaponObj, armorObj, consumablesObj, miscObj;
    private GameObject[] categoryObjects;
    private float objCategoryStartX = -90;

    private bool menuOpen = false;

    private const int NONE = 0, WEAPON = 1, ARMOR = 2, CONSUMABLES = 3, MISC = 4;

    public GameObject weaponStatsObject, armorStatsObject;

    public LeanTweenType tweenType;
    private float tweenTime = 0.5f;

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
                LeanTween.scale(obj, Vector3.zero, tweenTime).
                setEase(tweenType);
                LeanTween.moveLocalX(obj, objCategoryStartX, tweenTime).setEase(tweenType).
                    setOnComplete(() => ShowRequiredElements(NONE));
            }
        }

        // Toggle menu state
        menuOpen = !menuOpen;
    }

    public void ShowWeapon() {
        ShowRequiredElements(WEAPON);
        WeaponStatHolder holder = weaponStatsObject.GetComponent<WeaponStatHolder>();
        WeaponSO weapon = Inventory.Instance.equippedWeapon;
        Helper.Instance.SetupWeaponStats(holder, weapon);
    }

    public void ShowArmor() {
        ShowRequiredElements(ARMOR);
        ArmorStatHolder holder = armorStatsObject.GetComponent<ArmorStatHolder>();
        ArmorSO armor = Inventory.Instance.equippedArmor;
        Helper.Instance.SetupArmorStats(holder, armor);
    }

    private void ShowRequiredElements(int element) {
        // Hide row 2 elements
        weaponStatsObject.SetActive(element == WEAPON);
        armorStatsObject.SetActive(element == ARMOR);
    }
}
