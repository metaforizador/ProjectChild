using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDCanvas : MonoBehaviour {
    // Player
    public Image hpBar, shieldBar, staminaBar;
    public TextMeshProUGUI ammoText, playerLevel;

    // Enemy
    public Image enemyHpBar, enemyShieldBar;
    public TextMeshProUGUI enemyLevel;

    public void AdjustHUDBar(Image bar, float amount) {
        bar.fillAmount = amount / 100;
    }

    public void AdjustAmmoAmount(int max, float current) {
        int currentAmmo = (int)Mathf.Floor(max * (current / 100));
        ammoText.text = $"{currentAmmo}/{max}";
    }

    public void AdjustPlayerLevel(int level) {
        playerLevel.text = level.ToString();
    }

    public void ShowEnemyStats(Enemy enemy) {

    }
}
