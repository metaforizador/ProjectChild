using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterParent : MonoBehaviour {

    public enum CharacterType { Player, Enemy };

    // Temporary stats
    protected float hp, shield, stamina, ammo;

    protected float shieldRecovery, staminaRecovery, ammoRecovery, dodgeRate, criticalRate,
        rareItemFindRate, piercingDmg, kineticDmg, energyDmg, piercingRes, kineticRes, energyRes,
        attackSpd, movementSpd, fireRate;

    protected bool alive;
    public bool shooting;

    private const float MAX_VALUE = 100;

    protected CharacterType characterType;

    // CHANGE LATER WHEN WEAPONS ARE IMPLEMENTED
    private float weaponDamage = 50;
    private DamageType weaponType = DamageType.Piercing;
    public GameObject weaponBullet;
    private GameObject bulletPoint;
    private float weaponBulletSpeed = 100;
    private float weaponFireSpeed = 0.5f; // Seconds

    public virtual void Start() {
        // Retrieve bullet point
        bulletPoint = transform.Find("BulletPoint").gameObject;

        hp = MAX_VALUE;
        shield = MAX_VALUE;
        stamina = MAX_VALUE;
        ammo = MAX_VALUE;

        ResetValues();
    }

    /// <summary>
    /// Resets necessary values when spawning.
    /// </summary>
    private void ResetValues() {
        alive = true;

        hp = MAX_VALUE;
        shield = MAX_VALUE;
        stamina = MAX_VALUE;
        ammo = MAX_VALUE;

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
            if (shield < MAX_VALUE) {
                shield += shieldRecovery;

                if (shield > MAX_VALUE)
                    shield = MAX_VALUE;
            }

            // Recover stamina
            if (stamina < MAX_VALUE) {
                stamina += staminaRecovery;

                if (stamina > MAX_VALUE)
                    stamina = MAX_VALUE;
            }

            // Recover ammo
            if (ammo < MAX_VALUE) {
                ammo += ammoRecovery;

                if (ammo > MAX_VALUE)
                    ammo = MAX_VALUE;
            }

            yield return new WaitForSeconds(Stat.RECOVERY_DELAY);
        }
    }

    IEnumerator Shooting() {
        while (alive) {
            if (shooting) {
                GameObject thisBullet = Instantiate(weaponBullet);
                float damage = CalculateBulletDamage();
                thisBullet.GetComponent<bulletController>().Initialize(characterType, damage, weaponType);
             
                thisBullet.transform.position = bulletPoint.transform.position;
                thisBullet.transform.rotation = bulletPoint.transform.rotation;
                thisBullet.GetComponent<Rigidbody>().velocity = transform.forward.normalized * weaponBulletSpeed;
                yield return new WaitForSeconds(weaponFireSpeed);
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
        if (shield > 0) {
            shield -= amount;

            if (shield < 0) {
                rest = Mathf.Abs(shield);
                shield = 0;
            }
        }

        hp -= rest;

        if (hp <= 0)
            Die();
    }

    private void Die() {
        alive = false;
        hp = 0;
        shield = 0;

        Destroy(gameObject); // Destroy for now
    }
}
