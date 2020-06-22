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
    public GameObject enemyObject;
    public Image enemyHpBar, enemyShieldBar;
    public TextMeshProUGUI enemyLevel;
    private Enemy currentEnemy;
    private bool enemyAlive;
    private float hideEnemyStatsTime = 2f;

    public void Update() {
        // Hp and shield needs to be updated all the time in case they get recovered
        if (enemyAlive) {
            enemyShieldBar.fillAmount = currentEnemy.SHIELD / 100;
            enemyHpBar.fillAmount = currentEnemy.HP / 100;

            // If enemy health is 0, hide stats after some time
            if (currentEnemy.HP == 0) {
                enemyAlive = false;
                enemyHpBar.fillAmount = 0;
                enemyShieldBar.fillAmount = 0;
                Invoke("HideEnemyStats", hideEnemyStatsTime);
            }
        }
    }

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
    }

    private void HideEnemyStats() {
        enemyObject.SetActive(false);
    }
}
