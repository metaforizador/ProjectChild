using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterParent {

    [SerializeField]
    private EnemySO scriptableObject;

    private Player player;

    private float fireCounter;
    public float firingSpeed;
    public float bulletSpeed;
    public GameObject bulletPoint;
    public GameObject bullet;
    public float turnSpeed;
    private float turnSmoothVelocity;
    private float turnSmoothTime = 0.1f;

    public override void Start() {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        // Calculate stats from scriptableObject values
        shieldRecovery = Stat.CalculateValue(Stat.RECOVERY_MIN_SPEED, Stat.RECOVERY_MAX_SPEED, scriptableObject.shieldRecovery);
        staminaRecovery = Stat.CalculateValue(Stat.RECOVERY_MIN_SPEED, Stat.RECOVERY_MAX_SPEED, scriptableObject.staminaRecovery);
        ammoRecovery = Stat.CalculateValue(Stat.RECOVERY_MIN_SPEED, Stat.RECOVERY_MAX_SPEED, scriptableObject.ammoRecovery);

        dodgeRate = Stat.CalculateValue(Stat.DODGE_MIN_PERCENT, Stat.DODGE_MAX_PERCENT, scriptableObject.dodgeRate);
        criticalRate = Stat.CalculateValue(Stat.CRITICAL_MIN_PERCENT, Stat.CRITICAL_MAX_PERCENT, scriptableObject.criticalRate);

        piercingDmg = Stat.CalculateValue(Stat.DAMAGE_MIN_BOOST, Stat.DAMAGE_MAX_BOOST, scriptableObject.piercingDmg);
        kineticDmg = Stat.CalculateValue(Stat.DAMAGE_MIN_BOOST, Stat.DAMAGE_MAX_BOOST, scriptableObject.kineticDmg);
        energyDmg = Stat.CalculateValue(Stat.DAMAGE_MIN_BOOST, Stat.DAMAGE_MAX_BOOST, scriptableObject.energyDmg);

        piercingRes = Stat.CalculateValue(Stat.RESISTANCE_MIN_PERCENT, Stat.RESISTANCE_MAX_PERCENT, scriptableObject.piercingRes);
        kineticRes = Stat.CalculateValue(Stat.RESISTANCE_MIN_PERCENT, Stat.RESISTANCE_MAX_PERCENT, scriptableObject.kineticRes);
        energyRes = Stat.CalculateValue(Stat.RESISTANCE_MIN_PERCENT, Stat.RESISTANCE_MAX_PERCENT, scriptableObject.energyRes);

        attackSpd = Stat.CalculateValue(Stat.ATTACK_MIN_SPEED, Stat.ATTACK_MAX_SPEED, scriptableObject.attackSpd);
        movementSpd = Stat.CalculateValue(Stat.MOVEMENT_MIN_SPEED, Stat.MOVEMENT_MAX_SPEED, scriptableObject.movementSpd);
        fireRate = Stat.CalculateValue(Stat.FIRE_RATE_MIN_SPEED, Stat.FIRE_RATE_MAX_SPEED, scriptableObject.fireRate);

        base.Start();
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("PlayerBullet")) {
            float damage;
            DamageType type;
            player.CalculateBulletDamage(out type, out damage);
            TakeDamage(type, damage);
        }
    }

    private void Update()
    {
        //Shooting mechanics

        //if shooting
        if (true)
        {
            fireCounter -= Time.deltaTime;

            //enemy turns towards player while shooting
            Vector3 targetDirection = GameObject.Find("Player").transform.position - transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, turnSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }

        if (fireCounter < 0)
        {
            GameObject thisBullet = Instantiate(bullet);
            thisBullet.transform.position = bulletPoint.transform.position;
            thisBullet.transform.rotation = bulletPoint.transform.rotation;
            thisBullet.GetComponent<Rigidbody>().velocity = transform.forward.normalized * bulletSpeed;

            fireCounter = firingSpeed;
        }

        //resets firing interval counter after shooting
        //if (animator.GetBool("Shooting") == false)
        //{
        //    fireCounter = 0;
        //}
    }
}
