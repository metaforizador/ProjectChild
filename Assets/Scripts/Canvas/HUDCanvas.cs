using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Displays everything that is inside the player HUD.
/// </summary>
public class HUDCanvas : MonoBehaviour {
    // Player
    public Image hpBar, shieldBar, staminaBar, xpBar;
    public GameObject levelUpButton;
    public TextMeshProUGUI ammoText, playerLevel, levelUpPoint;

    // Enemy
    public GameObject enemyObject;
    public Image enemyHpBar, enemyShieldBar;
    public TextMeshProUGUI enemyLevel;
    private Enemy currentEnemy;
    private bool enemyAlive;
    private const float HIDE_ENEMY_STATS_TIME = 2f;

    // Interact
    public GameObject interactObject;
    public TextMeshProUGUI interactText;
    public LeanTweenType tweenType;
    private float tweenTime = 0.5f;
    private float hideInteractYPos = -150f;
    public const int CHEST_OPEN = 0, CHEST_CLOSE = 1;
    private string[] interacts = new string[] { "F - Open", "F - Close" };

    private UIAnimator animator;

    void Awake() {
        animator = CanvasMaster.Instance.uiAnimator;
        // Hide interact
        interactObject.transform.LeanMoveLocalY(hideInteractYPos, 0f);
    }

    public void Update() {
        // Hp and shield needs to be updated all the time in case they get recovered
        if (enemyAlive) {
            AdjustHUDBar(enemyHpBar, currentEnemy.HP);
            AdjustHUDBarShield(enemyShieldBar, currentEnemy.maxShield, currentEnemy.SHIELD);

            // If enemy health is 0, reset texts
            if (currentEnemy.HP == 0) {
                enemyAlive = false;
                enemyHpBar.fillAmount = 0;
                enemyShieldBar.fillAmount = 0;
            }
        }
    }

    /// <summary>
    /// Adjusts the provided hud bar by the provided amount.
    /// </summary>
    /// <param name="bar">hud bar image fill to adjust</param>
    /// <param name="amount">percentage amount to fill</param>
    public void AdjustHUDBar(Image bar, float amount) {
        bar.fillAmount = amount / 100;
    }

    /// <summary>
    /// Adjusts the shield bar 
    /// </summary>
    /// <param name="maxShield"></param>
    /// <param name="curShield"></param>
    public void AdjustHUDBarShield(Image bar, float maxShield, float curShield) {
        bar.fillAmount = curShield / maxShield;
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

    public void CheckRedeemableLevelPoints() {
        int points = PlayerStats.Instance.redeemableLevelPoints;

        if (points > 0 ) {
            levelUpButton.SetActive(true);
            levelUpPoint.text = points.ToString();
        } else {
            levelUpButton.SetActive(false);
        }
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
        // Disable 'enemyAlive' so update doesn't have to be called
        enemyAlive = false;
        enemyObject.SetActive(false);
    }

    public void InteractText(int type) {
        interactText.text = interacts[type];
    }

    public void ShowInteract(int type) {
        interactObject.SetActive(true);
        InteractText(type);
        animator.MoveY(interactObject, 0, tweenTime, tweenType);
    }

    public void HideInteract() {
        animator.MoveY(interactObject, hideInteractYPos, tweenTime, tweenType);
    }
}
