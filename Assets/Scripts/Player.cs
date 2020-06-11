using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Temporary stats
    [SerializeField]
    private Stat hp, shield, stamina, ammo;

    [SerializeField]
    private bool alive;

    private const float MAX_VALUE = 100;

    private PlayerStats stats;

    private Stat[] recoveries, recoveryMultipliers;
    private float recoveryDelay = 0.2f;
    private float recoveryBaseValue;
    private const float RECOVERY_MIN_SPEED = 1, RECOVERY_MAX_SPEED = 4;

    void Start() {
        hp = new Stat(MAX_VALUE);
        shield = new Stat(MAX_VALUE);
        stamina = new Stat(MAX_VALUE);
        ammo = new Stat(MAX_VALUE);

        stats = PlayerStats.Instance;

        recoveries = new Stat[] { shield, stamina, ammo };
        recoveryMultipliers = new Stat[] { stats.shieldRecovery, stats.staminaRecovery, stats.ammoRecovery };
        recoveryBaseValue = (RECOVERY_MAX_SPEED - RECOVERY_MIN_SPEED) / PlayerStats.MAX_BASE_STAT_VALUES;

        ResetValues();
    }

    private void ResetValues() {
        alive = true;

        hp.value = MAX_VALUE;
        shield.value = MAX_VALUE;
        stamina.value = MAX_VALUE;
        ammo.value = MAX_VALUE;

        StartCoroutine(RestoreRecoveries());
    }

    /// <summary>
    /// Restores recoverable value according to provided time delay.
    /// </summary>
    /// <returns>IEnumerator</returns>
    IEnumerator RestoreRecoveries() {
        while (alive) {
            for (int i = 0; i < recoveries.Length; i++) {
                Stat recovery = recoveries[i];

                // If value is not full
                if (recovery.value < MAX_VALUE) {
                    // Get the level of multiplier from PlayerStats
                    float multiplier = recoveryMultipliers[i].value;

                    recovery.value += RECOVERY_MIN_SPEED + (recoveryBaseValue * multiplier);

                    if (recovery.value > MAX_VALUE)
                        recovery.value = MAX_VALUE;
                }
            }

            yield return new WaitForSeconds(recoveryDelay);
        }
    }

    private void Die() {
        alive = false;
    }
}
