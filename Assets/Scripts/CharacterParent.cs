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

    protected CharacterType characterType;

    private HUDCanvas hud;

    [SerializeField]
    private WeaponSO weapon;

    // Weapon values
    private float weaponDamage;
    private DamageType weaponType;
    private float weaponBulletSpeed;
    private float weaponBulletConsumption;
    private float weaponRateOfFire;
    // Weapon prefab stuff
    public GameObject weaponBullet;
    private GameObject bulletPoint;

    public virtual void Start() {
        hud = CanvasMaster.Instance.HUDCanvas.GetComponent<HUDCanvas>();
        // Retrieve bullet point
        bulletPoint = transform.Find("BulletPoint").gameObject;

        RetrieveWeaponValues();
        ResetValues();
    }

    private void RetrieveWeaponValues() {
        weaponDamage = weapon.damagePerBullet;
        weaponType = weapon.weaponType;
        weaponBulletSpeed = weapon.bulletSpeed;
        weaponBulletConsumption = 100 / weapon.ammoSize; // Gets percentage
        weaponRateOfFire = weapon.rateOfFire;
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

    /// <summary>
    /// Restores recoverable value according to provided time delay.
    /// </summary>
    /// <returns>IEnumerator</returns>
    IEnumerator RestoreRecoveries() {
        while (alive) {
            // Recover shield
            if (SHIELD < MAX_VALUE) {
                SHIELD += shieldRecovery;

                if (SHIELD > MAX_VALUE)
                    SHIELD = MAX_VALUE;
            }

            // Recover stamina
            if (STAMINA < MAX_VALUE) {
                STAMINA += staminaRecovery;

                if (STAMINA > MAX_VALUE)
                    STAMINA = MAX_VALUE;
            }

            // Recover ammo
            if (AMMO < MAX_VALUE) {
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
                AMMO -= weaponBulletConsumption;
                GameObject thisBullet = Instantiate(weaponBullet);
                float damage = CalculateBulletDamage();
                thisBullet.GetComponent<bulletController>().Initialize(characterType, damage, weaponType);

                thisBullet.transform.position = bulletPoint.transform.position;
                thisBullet.transform.rotation = bulletPoint.transform.rotation;
                thisBullet.GetComponent<Rigidbody>().velocity = transform.forward.normalized * weaponBulletSpeed;

                yield return new WaitForSeconds(weaponRateOfFire / fireRate); // Shorten delay by fire rate
            }
            yield return 0;
        }
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

    public void TakeDamage(DamageType type, float amount) {
        // Check if damage got dodged
        if (Helper.CheckPercentage(dodgeRate)) {
            Debug.Log("Dodged");
            return;
        }

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

    private void Die() {
        alive = false;
        HP = 0;
        SHIELD = 0;

        Destroy(gameObject); // Destroy for now
    }
}
