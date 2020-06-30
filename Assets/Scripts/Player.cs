using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterParent {

    private PlayerStats stats;

    private Stat[] recoveryStats;

    // Testing purposes
    public float testDamageKeyU = 20;
    public int testXpKeyX = 20;

    public override void Start() {
        characterType = CharacterType.Player;
        stats = PlayerStats.Instance;

        RefreshStats();

        base.Start();

        // Show player's hud
        hud.gameObject.SetActive(true);
    }

    public void RefreshStats() {
        shieldRecovery = stats.shieldRecovery.currentValue;
        staminaRecovery = stats.staminaRecovery.currentValue;
        ammoRecovery = stats.ammoRecovery.currentValue;

        dodgeRate = stats.dodgeRate.currentValue;
        criticalRate = stats.criticalRate.currentValue;
        rareItemFindRate = stats.rareItemFindRate.currentValue;

        piercingDmg = stats.piercingDmg.currentValue;
        kineticDmg = stats.kineticDmg.currentValue;
        energyDmg = stats.energyDmg.currentValue;

        piercingRes = stats.piercingRes.currentValue;
        kineticRes = stats.kineticRes.currentValue;
        energyRes = stats.energyRes.currentValue;

        attackSpd = stats.attackSpd.currentValue;
        movementSpd = stats.movementSpd.currentValue;
        fireRate = stats.fireRate.currentValue;

        // Some armor values are affected by stats
        RetrieveArmorValues();
    }

    protected override void Update() {
        base.Update();
        // Test taking damage
        if (Input.GetKeyDown(KeyCode.U)) {
            TakeDamage(DamageType.Piercing, testDamageKeyU, 0);
        }
        
        // Test gaining xp
        if (Input.GetKeyDown(KeyCode.X)) {
            stats.GainXP(testXpKeyX);
        }
    }

    void OnDestroy() {
        hud.gameObject.SetActive(false); // Hide player's hud on destroy
    }

    /**************** Collisions ****************/

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Bullet")) {
            bulletController bullet = collision.gameObject.GetComponent<bulletController>();
            if (bullet.shooter == CharacterType.Enemy) {
                TakeDamage(bullet.damageType, bullet.damage, bullet.criticalRate);
            }
        }
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Chest")) {
            hud.ShowInteract(HUDCanvas.CHEST);
        }
    }

    void OnTriggerStay(Collider collider) {
        if (collider.CompareTag("Chest")) {
            if (Input.GetButtonDown("Interact")) {
                // Open chest
                Debug.Log("Trigger press");
            }
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider.CompareTag("Chest")) {
            hud.HideInteract();
        }
    }
}
