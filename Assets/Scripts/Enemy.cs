using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterParent {

    [SerializeField]
    private EnemySO scriptableObject = null;

    private Player player;

    public Animator animator;

    //public enum State {Patrolling, Shooting, Dying};
    private AI.State curState;
    public AI.State startState;

    private List<Vector3> waypoints;

    public int level { get; private set; }

    public float turnSpeed;
    private float turnSmoothVelocity;
    private float turnSmoothTime = 0.1f;

    public override void Start() {
        //transitions to startState
        TransitionToState(startState);

        //finds pathfinding routes -- IN TESTING
        GameObject route = GameObject.Find("Route1");
        waypoints = new List<Vector3>();

        foreach (Transform child in route.transform)
        {
            waypoints.Add(child.position);
        }

        //
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        characterType = CharacterType.Enemy;

        animator = GetComponentInChildren<Animator>();

        // Calculate stats from scriptableObject values
        shieldRecovery = Stat.CalculateValue(Stat.RECOVERY_MIN_SPEED, Stat.RECOVERY_MAX_SPEED, scriptableObject.shieldRecovery);
        staminaRecovery = Stat.CalculateValue(Stat.RECOVERY_MIN_SPEED, Stat.RECOVERY_MAX_SPEED, scriptableObject.staminaRecovery);

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

        // If weapon or armor is not assigned and EnemySO values are not null
        if (GetWeapon() == null && scriptableObject.startingWeapon != null)
            ChangeWeapon(scriptableObject.startingWeapon);
        if (GetArmor() == null && scriptableObject.startingArmor != null)
            ChangeArmor(scriptableObject.startingArmor);
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Bullet")) {
            bulletController bullet = collision.gameObject.GetComponent<bulletController>();
            if (bullet.shooter == CharacterType.Player) {
                TakeDamage(bullet.damageType, bullet.damage, bullet.criticalRate);
            }
        }
    }

    protected override void TakeDamage(DamageType type, float amount, float criticalRate) {
        base.TakeDamage(type, amount, criticalRate);
        hud.ShowEnemyStats(this);
    }

    protected override void Update() {

        base.Update();
        // Don't run update if enemy is not alive
        if (!alive)
        {
            return;
        }

        curState.UpdateState(this, Time.deltaTime);
    }

    public bool TransitionToState(AI.State newState)
    {
        if (curState != newState)
        {
            Debug.Log("State transition " + curState + " -> " + newState);

            OnStateExit(curState);
            curState = newState;
            OnStateEnter(newState);

            return true;
        }

        return false;
    }

    private void OnStateEnter(AI.State newState)
    {
        
    }

    private void OnStateExit(AI.State curState)
    {
        shooting = false;
        animator.SetBool("Shooting", false);
        animator.SetBool("Moving", false);
    }

    public void Shoot()
    {
        shooting = true;
        animator.SetBool("Shooting", true);

        //enemy turns towards player while shooting
        Vector3 targetDirection = GameObject.Find("Player").transform.position - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, turnSpeed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    public void Patrol()
    {
        Vector3 closestPoint = new Vector3(); //siirrettävä onStateEnteriin

        if(closestPoint == new Vector3(0, 0, 0))
        {
            closestPoint = FindClosestPoint();
        }

        foreach(Vector3 waypoint in waypoints)
        {
            //kun enemy pääsee waypointille, lähde kulkemaan seuraavaa waypointtia kohti indexin mukaan
        }

        //disabled for demo// MoveTowards(closestPoint);
    }

    private Vector3 FindClosestPoint()
    {
        Vector3 closestPoint = new Vector3();

        foreach (Vector3 waypoint in waypoints)
        {
            float closestDist = Vector3.Magnitude(closestPoint - transform.position);
            float distance = Vector3.Magnitude(waypoint - transform.position);

            if (closestDist == 0 || closestDist > distance)
            {
                closestPoint = waypoint;
            }
        }

        return closestPoint;
    }

    public void MoveTowards(Vector3 targetPoint)
    {
        animator.SetBool("Moving", true);

        Vector3 targetDirection = targetPoint - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, turnSpeed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);

        Debug.Log(movementSpd);

        transform.position += transform.forward * movementSpd * 40 * Time.deltaTime; //movementSpd should be multiplied with base movement speed somewhere else
    }

    protected override void Die() {
        PlayerStats.Instance.GainXP(scriptableObject.xp);
        base.Die();
    }
}
