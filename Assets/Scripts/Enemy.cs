using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterParent {

    [SerializeField]
    private EnemySO scriptableObject;

    private Player player;

    private enum State {Patrolling, Shooting, Dying};
    private State curState = State.Patrolling;

    public int level { get; private set; }

    public float turnSpeed;
    private float turnSmoothVelocity;
    private float turnSmoothTime = 0.1f;

    public override void Start() {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        characterType = CharacterType.Enemy;

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

        level = scriptableObject.level;

        base.Start();
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Bullet")) {
            bulletController bullet = collision.gameObject.GetComponent<bulletController>();
            if (bullet.shooter == CharacterType.Player) {
                TakeDamage(bullet.damageType, bullet.damage);
            }
        }
    }

    protected override void TakeDamage(DamageType type, float amount) {
        base.TakeDamage(type, amount);
        hud.ShowEnemyStats(this);
    }

    private void Update()
    {
        // Don't run update if enemy is not alive
        if (!alive)
            return;

        //Shooting mechanics

        // If player is close enough and enemy is able to see the player
        if (true) {
            curState = State.Shooting;
            shooting = true;
        } else {
            curState = State.Patrolling;
            shooting = false;
        }

        //if shooting
        if (curState == State.Shooting)
        {
            //enemy turns towards player while shooting
            Vector3 targetDirection = GameObject.Find("Player").transform.position - transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, turnSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }

        //resets firing interval counter after shooting
        //if (animator.GetBool("Shooting") == false)
        //{
        //    fireCounter = 0;
        //}
    }

    protected override void Die() {
        PlayerStats.Instance.GainXP(scriptableObject.xp);
        base.Die();
    }
}
