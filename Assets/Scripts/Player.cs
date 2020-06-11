using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    /// <summary>
    /// Holds temporary stat values.
    /// 
    /// This class is needed so that stat values can be passed as
    /// a reference and not as a value.
    /// </summary>
    [System.Serializable]
    class TempStat {
        public float value;

        public TempStat(float value) {
            this.value = value;
        }
    }

    // Temporary stats
    [SerializeField]
    private TempStat hp, shield, stamina, ammo;

    [SerializeField]
    private bool alive;

    private const float MAX_VALUE = 100;

    private PlayerStats stats;

    private Stat[] recoveryStats;
    private TempStat[] recoveries;
    private float recoveryDelay = 0.2f;

    void Start() {
        hp = new TempStat(MAX_VALUE);
        shield = new TempStat(MAX_VALUE);
        stamina = new TempStat(MAX_VALUE);
        ammo = new TempStat(MAX_VALUE);

        stats = PlayerStats.Instance;

        recoveries = new TempStat[] { shield, stamina, ammo };
        recoveryStats = new Stat[] { stats.shieldRecovery, stats.staminaRecovery, stats.ammoRecovery };

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
                TempStat recovery = recoveries[i];

                // If value is not full
                if (recovery.value < MAX_VALUE) {
                    // Get minimum, level and valuerPerLevel from stat
                    Stat stat = recoveryStats[i];

                    recovery.value += stat.realMinValue + (stat.valuePerLevel * stat.level);

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
