using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterParent {

    // References to classes
    private GameMaster gm;
    private CanvasMaster cm;
    private HotbarCanvas hotbar;

    private Inventory inventory;
    private PlayerStats stats;

    private Stat[] recoveryStats;

    // Testing purposes
    public float testDamageKeyU = 20;
    public int testXpKeyX = 100;

    // Checking trigger presses to avoid "input not always registering"
    private Collider triggerCollider;

    public override void Start() {
        // Retrieve references
        gm = GameMaster.Instance;
        cm = CanvasMaster.Instance;
        hotbar = cm.hotbarCanvas.GetComponent<HotbarCanvas>();

        GameMaster.Instance.SetState(GameState.Movement);
        characterType = CharacterType.Player;
        stats = PlayerStats.Instance;
        inventory = Inventory.Instance;
        stats.player = this;    // Add this player to singleton variable for better access

        // Get weapon and stats from the singleton stats
        ChangeWeapon(inventory.equippedWeapon);
        ChangeArmor(inventory.equippedArmor);

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

    public override WeaponSO ChangeWeapon(WeaponSO weapon) {
        inventory.equippedWeapon = weapon;  // Add weapon to singleton inventory
        return base.ChangeWeapon(weapon);
    }

    public override ArmorSO ChangeArmor(ArmorSO armor) {
        inventory.equippedArmor = armor;    // Add armor to singleton inventory
        return base.ChangeArmor(armor);
    }

    protected override void Update() {
        base.Update();

        bool inputEnabled = gm.gameState.Equals(GameState.Movement);

        // Check trigger interact presses
        if (triggerCollider != null && Input.GetButtonDown("Interact") && inputEnabled) {
            if (triggerCollider.CompareTag("Chest")) {
                triggerCollider.GetComponent<Chests>().OpenChest();
            }
        }

        // Debug tests
        if (Application.isEditor) {
            // Test taking damage
            if (Input.GetKeyDown(KeyCode.U) && inputEnabled) {
                TakeDamage(DamageType.Piercing, testDamageKeyU, 0);
            }

            // Test gaining xp
            if (Input.GetKeyDown(KeyCode.X) && inputEnabled) {
                stats.GainXP(testXpKeyX);
            }
        }

        // Check hotbar presses
        bool hotbarInputEnabled = gm.gameState.Equals(GameState.Movement) ||
            gm.gameState.Equals(GameState.Menu) || gm.gameState.Equals(GameState.Hotbar);

        if (hotbarInputEnabled) {
            int hotbarButtonAmount = hotbar.hotbarButtonAmount;
            for (int i = 1; i <= hotbarButtonAmount; ++i) {
                if (Input.GetKeyDown("" + i)) {
                    hotbar.HotbarButtonClicked(i - 1); // index is 1 lower than button number
                }
            }
        }
    }

    public void UseConsumable(ConsumableSO consumable) {
        TopInfoCanvas info = CanvasMaster.Instance.topInfoCanvas;

        switch (consumable.consumableType) {
            /************ BATTERY ************/
            case ConsumableType.Battery:
                consumable.DetermineFinalBatteryType();
                switch (consumable.batteryType) {
                    case ConsumableSO.BatteryType.Shield:
                        float amount = consumable.shieldRecoveryPercentage;
                        info.ShowShieldRecoveredText(amount);
                        SHIELD += amount;
                        break;
                    case ConsumableSO.BatteryType.Stamina:
                        float staminaAmount = consumable.boostStaminaRecoverySpeed;
                        float staminaTime = consumable.boostTimeInSeconds;
                        info.ShowBoostText("stamina", staminaAmount, staminaTime);
                        BoostRecovery(B_STAMINA, staminaAmount, staminaTime);
                        break;
                    case ConsumableSO.BatteryType.Ammo:
                        float ammoAmount = consumable.boostAmmoRecoverySpeed;
                        float ammoTime = consumable.boostTimeInSeconds;
                        info.ShowBoostText("ammo", ammoAmount, ammoTime);
                        BoostRecovery(B_AMMO, ammoAmount, ammoTime);
                        break;
                }
                break;
            /************ COMSAT LINK ************/
            case ConsumableType.ComsatLink:
                if (consumable.CheckIfUsageSuccessful()) {
                    // Call an airstrike
                }
                break;
            /************ RIG ************/
            case ConsumableType.Rig:
                if (consumable.CheckIfUsageSuccessful()) {
                    info.ShowHealthRecoveredText();
                    HP += ConsumableSO.RIG_HP_TO_RECOVER_PERCENTAGE;
                }
                break;
        }
    }

    protected override void Die() {
        base.Die();
        cm.ShowGameOverCanvas(true);
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
        triggerCollider = collider;

        if (collider.CompareTag("Chest")) {
            hud.ShowInteract(HUDCanvas.CHEST_OPEN);
        }
    }

    void OnTriggerExit(Collider collider) {
        triggerCollider = null;

        if (collider.CompareTag("Chest")) {
            hud.HideInteract();
            cm.chestCanvas.CloseChest();
        }
    }
}
