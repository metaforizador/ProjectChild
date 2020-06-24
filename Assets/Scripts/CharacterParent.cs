using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterParent : MonoBehaviour {

    public enum CharacterType { Player, Enemy };

    // Temporary stats
    private float hp, shield, stamina, ammo;

    public float HP { get { return hp; }
        private set {
            hp = value;
            if (characterType == CharacterType.Player)
                hud.AdjustHUDBar(hud.hpBar, hp);
        }
    }

    public float SHIELD {
        get { return shield; }
        private set {
            shield = value;
            if (characterType == CharacterType.Player)
                hud.AdjustHUDBar(hud.shieldBar, shield);
        }
    }

    public float STAMINA {
        get { return stamina; }
        private set {
            stamina = value;
            if (characterType == CharacterType.Player)
                hud.AdjustHUDBar(hud.staminaBar, stamina);
        }
    }

    public float AMMO {
        get { return ammo; }
        private set {
            ammo = value;
            if (characterType == CharacterType.Player)
                hud.AdjustAmmoAmount(weapon.ammoSize, ammo);
        }
    }

    protected float shieldRecovery, staminaRecovery, ammoRecovery, dodgeRate, criticalRate,
        rareItemFindRate, piercingDmg, kineticDmg, energyDmg, piercingRes, kineticRes, energyRes,
        attackSpd, movementSpd, fireRate;

    protected bool alive;
    public bool shooting;

    private const float MAX_VALUE = 100;

    // Recovery delay values
    private const float RECOVERY_DELAY_TIME = 1f;
    private const int D_SHIELD = 0, D_STAMINA = 1;
    private float[] delays = new float[] { 0, 0 };

    protected CharacterType characterType;

    protected HUDCanvas hud;

    [SerializeField]
    private WeaponSO weapon = null;

    [SerializeField]
    private ArmorSO armor = null;

    // Weapon values
    private float weaponDamage;
    private DamageType weaponType;
    private float weaponBulletSpeed;
    private float weaponBulletConsumption;
    private float weaponRateOfFire;
    private float weaponReloadTime;
    // Weapon prefab stuff
    public GameObject weaponBullet;
    private GameObject bulletPoint;

    // Armor values
    private float decreaseShieldRecoveryDelay;
    private int increaseShield;
    private float decreaseOpponentCriticalRate;
    private float decreaseOpponentCriticalMultiplier;
    private float reduceMovementSpeed;
    private float reduceStaminaRecoveryRate;

    public virtual void Start() {
        hud = CanvasMaster.Instance.HUDCanvas.GetComponent<HUDCanvas>();
        // Retrieve bullet point
        bulletPoint = transform.Find("BulletPoint").gameObject;

        RetrieveWeaponValues();
        RetrieveArmorValues();
        ResetValues();
    }

    private void RetrieveWeaponValues() {
        weaponDamage = weapon.damagePerBullet;
        weaponType = weapon.weaponType;
        weaponBulletSpeed = weapon.bulletSpeed;
        weaponBulletConsumption = 100 / weapon.ammoSize; // Gets percentage
        weaponRateOfFire = weapon.rateOfFire;
        weaponReloadTime = weapon.reloadTime;
    }

    private void RetrieveArmorValues() {
        if (armor != null) {
            decreaseShieldRecoveryDelay = armor.decreaseShieldRecoveryDelay;
            increaseShield = armor.increaseShield;
            decreaseOpponentCriticalRate = armor.decreaseOpponentCriticalRate;
            decreaseOpponentCriticalMultiplier = armor.decreaseOpponentCriticalMultiplier;
            reduceMovementSpeed = armor.reduceMovementSpeed;
            reduceStaminaRecoveryRate = armor.reduceStaminaRecoveryRate;
        }
    }

    /// <summary>
    /// Resets necessary values when spawning.
    /// </summary>
    private void ResetValues() {
        alive = true;

        HP = MAX_VALUE;
        SHIELD = MAX_VALUE;
        STAMINA = MAX_VALUE;
        AMMO = MAX_VALUE;

        StartCoroutine(RestoreRecoveries());
        StartCoroutine(Shooting());
    }

    protected virtual void Update() {
        for (int i = 0; i < delays.Length; i++) {
            if (delays[i] > 0)
                delays[i] -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Restores recoverable value according to provided time delay.
    /// </summary>
    /// <returns>IEnumerator</returns>
    IEnumerator RestoreRecoveries() {
        while (alive) {
            // Recover shield
            if (SHIELD < MAX_VALUE && delays[D_SHIELD] <= 0) {
                SHIELD += shieldRecovery;

                if (SHIELD > MAX_VALUE)
                    SHIELD = MAX_VALUE;
            }

            // Recover stamina
            if (STAMINA < MAX_VALUE && delays[D_STAMINA] <= 0) {
                STAMINA += staminaRecovery;

                if (STAMINA > MAX_VALUE)
                    STAMINA = MAX_VALUE;
            }

            // Recover ammo
            if (AMMO < MAX_VALUE && characterType == CharacterType.Player) {
                AMMO += ammoRecovery;

                if (AMMO > MAX_VALUE)
                    AMMO = MAX_VALUE;
            }

            yield return new WaitForSeconds(Stat.RECOVERY_DELAY);
        }
    }

    IEnumerator Shooting() {
        while (alive) {
            if (shooting && (AMMO >= weaponBulletConsumption)) {
                // Decrease ammo by bullet consumption amount
                AMMO -= weaponBulletConsumption;

                // Create the bullet, calculate damage and initialize necessary values
                GameObject thisBullet = Instantiate(weaponBullet);
                float damage = CalculateBulletDamage();
                thisBullet.GetComponent<bulletController>().Initialize(characterType, damage, weaponType);

                // Set bullets position and speed
                thisBullet.transform.position = bulletPoint.transform.position;
                thisBullet.transform.rotation = bulletPoint.transform.rotation;
                thisBullet.GetComponent<Rigidbody>().velocity = transform.forward.normalized * weaponBulletSpeed;

                // Enemies reload weapons when they run out of ammo
                if (characterType == CharacterType.Enemy && AMMO < weaponBulletConsumption)
                    Invoke("reloadAmmo", weaponReloadTime);

                yield return new WaitForSeconds(weaponRateOfFire / fireRate); // Shorten delay by fire rate
            }
            yield return 0;
        }
    }

    /// <summary>
    /// Needed for enemy to invoke ammo reload.
    /// </summary>
    private void reloadAmmo() {
        AMMO = 100;
    }

    private float CalculateBulletDamage() {
        float damageToCause = weaponDamage;

        // Add percentage to damage based on damage stats
        switch (weaponType) {
            case DamageType.Piercing:
                damageToCause *= piercingDmg;
                break;
            case DamageType.Kinetic:
                damageToCause *= kineticDmg;
                break;
            case DamageType.Energy:
                damageToCause *= energyDmg;
                break;
        }

        // Check if it was a critical hit
        if (Helper.CheckPercentage(criticalRate)) {
            Debug.Log("Critical hit");
            damageToCause *= Stat.CRITICAL_HIT_MULTIPLIER;
        }

        return damageToCause;
    }

    protected virtual void TakeDamage(DamageType type, float amount) {
        // Check if damage got dodged
        if (Helper.CheckPercentage(dodgeRate)) {
            Debug.Log("Dodged");
            return;
        }

        // Add delay to shield recovery
        delays[D_SHIELD] = RECOVERY_DELAY_TIME;

        // Calculate resistance to given damage type
        switch (type) {
            case DamageType.Piercing:
                amount -= amount * (piercingRes / 100);
                break;
            case DamageType.Kinetic:
                amount -= amount * (kineticRes / 100);
                break;
            case DamageType.Energy:
                amount -= amount * (energyRes / 100);
                break;
        }

        float rest = 0; // Damage left after hitting the shield
        if (SHIELD > 0) {
            SHIELD -= amount;

            if (SHIELD < 0) {
                rest = Mathf.Abs(SHIELD);
                SHIELD = 0;
            }
        }

        HP -= rest;

        if (HP <= 0)
            Die();
    }

    protected virtual void Die() {
        alive = false;
        HP = 0;
        SHIELD = 0;

        Destroy(gameObject); // Destroy for now
    }
}
