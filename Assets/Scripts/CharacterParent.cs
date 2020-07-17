using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Presets;

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
                hud.AdjustHUDBarShield(maxShield, SHIELD);
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
    public float maxShield { get; private set; }

    // Movement speed when taking armor values to account
    public float movementSpeedMultiplier { get; private set; }

    // Recovery delay values
    private const float RECOVERY_DELAY_TIME = 1.5f;
    private const int D_SHIELD = 0, D_STAMINA = 1;
    private float[] delays = new float[] { 0, 0 };

    protected CharacterType characterType;

    protected HUDCanvas hud;

    // Playing audio
    [SerializeField]
    private Preset audioPreset;
    private AudioSource audioSource;

    // Weapon and armor
    [SerializeField]
    private WeaponSO weapon = null;
    [SerializeField]
    private ArmorSO armor = null;

    public WeaponSO GetWeapon() => weapon;
    public ArmorSO GetArmor() => armor;

    // Weapon values
    private AudioClip weaponShootingSound;
    private AudioClip weaponReloadSound;
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
    private float armorDecreaseShieldRecoveryDelay;
    private float armorDecreaseOpponentCriticalRate;
    private float armorDecreaseOpponentCriticalMultiplier;
    private float armorReduceStaminaRecoveryRate;

    public virtual void Start() {
        hud = CanvasMaster.Instance.HUDCanvas.GetComponent<HUDCanvas>();
        // Create audio component and add preset
        audioSource = gameObject.AddComponent<AudioSource>();
        audioPreset.ApplyTo(audioSource);
        // Retrieve bullet point
        bulletPoint = transform.Find("BulletPoint").gameObject;

        RetrieveWeaponValues();
        RetrieveArmorValues();
        ResetValues();
    }

    private void RetrieveWeaponValues() {
        weaponShootingSound = weapon.shootingSound;
        weaponReloadSound = weapon.reloadingSound;
        weaponDamage = weapon.damagePerBullet;
        weaponType = weapon.weaponType;
        weaponBulletSpeed = weapon.bulletSpeed;
        weaponBulletConsumption = 100 / weapon.ammoSize; // Gets percentage
        weaponRateOfFire = weapon.rateOfFire;
        weaponReloadTime = weapon.reloadTime;
    }

    protected void RetrieveArmorValues() {
        maxShield = MAX_VALUE;
        movementSpeedMultiplier = movementSpd;

        if (armor != null) {
            armorDecreaseShieldRecoveryDelay = armor.decreaseShieldRecoveryDelay / 100;
            maxShield += armor.increaseShield;
            armorDecreaseOpponentCriticalRate = armor.decreaseOpponentCriticalRate;
            armorDecreaseOpponentCriticalMultiplier = armor.decreaseOpponentCriticalMultiplier / 100;
            movementSpeedMultiplier = movementSpd - ((armor.reduceMovementSpeed / 100) * movementSpd);
            armorReduceStaminaRecoveryRate = armor.reduceStaminaRecoveryRate;
        }
    }

    /// <summary>
    /// Resets necessary values when spawning.
    /// </summary>
    private void ResetValues() {
        alive = true;

        HP = MAX_VALUE;
        SHIELD = maxShield;
        STAMINA = MAX_VALUE;
        AMMO = MAX_VALUE;

        StartCoroutine(RestoreRecoveries());
        StartCoroutine(Shooting());
    }

    public virtual WeaponSO ChangeWeapon(WeaponSO weapon) {
        WeaponSO oldWeapon = this.weapon;
        this.weapon = weapon;
        RetrieveWeaponValues();

        return oldWeapon;
    }

    public virtual ArmorSO ChangeArmor(ArmorSO armor) {
        ArmorSO oldArmor = this.armor;
        this.armor = armor;
        RetrieveArmorValues();

        return oldArmor;
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
            if (SHIELD < maxShield && delays[D_SHIELD] <= 0) {
                SHIELD += shieldRecovery;

                if (SHIELD > maxShield)
                    SHIELD = maxShield;
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

                // Make a shooting sound
                //ERROR audioSource.PlayOneShot(weaponShootingSound);

                // Create the bullet, calculate damage and initialize necessary values
                GameObject thisBullet = Instantiate(weaponBullet);
                float damage = CalculateBulletDamage();
                thisBullet.GetComponent<bulletController>().Initialize(characterType, damage, criticalRate, weaponType);

                // Set bulletDirection towards crosshair point for the player / towards player for enemies
                Vector3 bulletDirection = transform.forward;

                if (characterType == CharacterType.Player)
                {
                    Vector3 crosshairPoint = new Vector3(0, 0, 0);

                    RaycastHit hit;
                    Ray ray = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenPointToRay(GameObject.Find("Crosshair").transform.position);

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~9))
                    {
                        crosshairPoint = hit.point;
                        bulletDirection = crosshairPoint - bulletPoint.transform.position;
                    }
                    else
                    {
                        crosshairPoint = ray.GetPoint(1000);
                        bulletDirection = crosshairPoint - bulletPoint.transform.position;
                    }
                }
                else if (characterType == CharacterType.Enemy)
                {
                    bulletDirection = GameObject.Find("Player").transform.position - bulletPoint.transform.position;
                }

                // Set bullets position and speed
                thisBullet.transform.position = bulletPoint.transform.position;
                thisBullet.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, bulletDirection, 100, 0.0f));
                thisBullet.GetComponent<Rigidbody>().velocity = bulletDirection.normalized * weaponBulletSpeed;

                // Enemies reload weapons when they run out of ammo
                if (characterType == CharacterType.Enemy && AMMO < weaponBulletConsumption) {
                    //ERROR audioSource.PlayOneShot(weaponReloadSound);
                    Invoke("reloadAmmo", weaponReloadTime);
                }

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

        return damageToCause;
    }

    protected virtual void TakeDamage(DamageType type, float amount, float criticalRate) {
        // Check if damage got dodged
        if (Helper.CheckPercentage(dodgeRate)) {
            Debug.Log("Dodged");
            return;
        }

        // Check if it was a critical hit
        if (Helper.CheckPercentage(criticalRate - armorDecreaseOpponentCriticalRate)) {
            Debug.Log("Critical hit");
            amount *= (Stat.CRITICAL_HIT_MULTIPLIER - armorDecreaseOpponentCriticalMultiplier);
        }

        // Add delay to shield recovery
        delays[D_SHIELD] = RECOVERY_DELAY_TIME - (RECOVERY_DELAY_TIME * armorDecreaseShieldRecoveryDelay);

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

        // If shield is left, take damage to shield
        if (SHIELD > 0) {
            SHIELD -= amount;

            // If shield went under 0, add the left over damage to hp
            if (SHIELD < 0) {
                amount = Mathf.Abs(SHIELD);
                SHIELD = 0;
            } else {
                amount = 0;
            }
        }

        HP -= amount;

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
