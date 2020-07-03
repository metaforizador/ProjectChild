using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCanvas : MonoBehaviour {

    public GameObject weaponObj, armorObj, consumablesObj, miscObj;
    private GameObject[] categoryObjects;

    private bool menuOpen = false;

    public LeanTweenType tweenType;
    private float tweenTime = 0.25f;

    private float objCategoryStartX = -90;
    private float objCategoryTargetScale = 0.7f;

    void Awake() {
        categoryObjects = new GameObject[] { weaponObj, armorObj, consumablesObj, miscObj };

        foreach (GameObject obj in categoryObjects) {
            obj.transform.LeanMoveLocalX(objCategoryStartX, 0f);
            obj.transform.localScale = Vector3.zero;
        }
    }

    public void ToggleMenu() {
        foreach (GameObject obj in categoryObjects) {
            if (!menuOpen) {
                // Open menu
                LeanTween.scale(obj, new Vector3(objCategoryTargetScale, objCategoryTargetScale, objCategoryTargetScale), tweenTime).
                setEase(tweenType);
                LeanTween.moveLocalX(obj, 0, tweenTime).setEase(tweenType);
            } else {
                // Close menu
                LeanTween.scale(obj, Vector3.zero, tweenTime).
                setEase(tweenType);
                LeanTween.moveLocalX(obj, objCategoryStartX, tweenTime).setEase(tweenType);
            }
        }

        // Toggle menu open boolean
        menuOpen = !menuOpen;
    }
}
