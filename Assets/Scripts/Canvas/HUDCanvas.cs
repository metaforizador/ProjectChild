using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDCanvas : MonoBehaviour {
    // Player
    public Image hpBar, shieldBar, staminaBar, xpBar;
    public TextMeshProUGUI ammoText, playerLevel;

    // Enemy
    public GameObject enemyObject;
    public Image enemyHpBar, enemyShieldBar;
    public TextMeshProUGUI enemyLevel;
    private Enemy currentEnemy;
    private bool enemyAlive;
    private const float HIDE_ENEMY_STATS_TIME = 2f;

    public void Update() {
        // Hp and shield needs to be updated all the time in case they get recovered
        if (enemyAlive) {
            enemyShieldBar.fillAmount = currentEnemy.SHIELD / currentEnemy.maxShield;
            enemyHpBar.fillAmount = currentEnemy.HP / 100;

            // If enemy health is 0, reset texts
            if (currentEnemy.HP == 0) {
                enemyAlive = false;
                enemyHpBar.fillAmount = 0;
                enemyShieldBar.fillAmount = 0;
            }
        }
    }

    public void AdjustHUDBar(Image bar, float amount) {
        bar.fillAmount = amount / 100;
    }

    public void AdjustHUDBarShield(float maxShield, float curShield) {
        shieldBar.fillAmount = curShield / maxShield;
    }

    public void AdjustHUDBarXP(float lastLevelXP, float nextLevelXP, float curXP) {
        nextLevelXP -= lastLevelXP;
        curXP -= lastLevelXP;
        xpBar.fillAmount = curXP / nextLevelXP;
    }

    public void AdjustAmmoAmount(int max, float current) {
        int currentAmmo = (int)Mathf.Floor(max * (current / 100));
        ammoText.text = $"{currentAmmo}/{max}";
    }

    public void AdjustPlayerLevel(int level) {
        playerLevel.text = level.ToString();
    }

    /// <summary>
    /// Shows enemy stats in hud.
    /// 
    /// When enemy gets hit, it calls this method.
    /// </summary>
    /// <param name="enemy">enemy which got hit</param>
    public void ShowEnemyStats(Enemy enemy) {
        // If enemy stats are not active, activate them
        if (!enemyObject.activeSelf)
            enemyObject.SetActive(true);

        enemyAlive = true;
        currentEnemy = enemy;
        
        // Level needs to be updated only once
        enemyLevel.text = currentEnemy.level.ToString();

        // Cancel possible "HideEnemyStats" Invoke if player shoots the enemy
        CancelInvoke();

        // Hide stats after some time of the latest hit
        Invoke("HideEnemyStats", HIDE_ENEMY_STATS_TIME);
    }

    private void HideEnemyStats() {
        enemyObject.SetActive(false);
    }
}
