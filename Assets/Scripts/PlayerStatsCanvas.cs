using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatsCanvas : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI shieldNum, staminaNum, ammoNum, dodgeNum, criticalNum, rareItemFindNum,
        piercingDmgNum, kineticDmgNum, energyDmgNum, piercingResNum, kineticResNum, energyResNum,
        attackNum, movementNum, fireRateNum, levelNum, xpNum, nextLevelNum;

    public void OnEnable() {
        PlayerStats p = PlayerStats.Instance;

        shieldNum.text = p.shieldRecovery.value.ToString();
        staminaNum.text = p.staminaRecovery.value.ToString();
        ammoNum.text = p.ammoRecovery.value.ToString();

        dodgeNum.text = p.dodgeRate.value.ToString();
        criticalNum.text = p.criticalRate.value.ToString();
        rareItemFindNum.text = p.rareItemFindRate.value.ToString();

        piercingDmgNum.text = p.piercingDmg.value.ToString();
        kineticDmgNum.text = p.kineticDmg.value.ToString();
        energyDmgNum.text = p.energyDmg.value.ToString();

        piercingResNum.text = p.piercingRes.value.ToString();
        kineticResNum.text = p.kineticRes.value.ToString();
        energyResNum.text = p.energyRes.value.ToString();

        attackNum.text = p.attackSpd.value.ToString();
        movementNum.text = p.movementSpd.value.ToString();
        fireRateNum.text = p.fireRate.value.ToString();

        levelNum.text = p.level.ToString();
        xpNum.text = p.xp.ToString();
        nextLevelNum.text = p.nextLevelUpXp.ToString();
    }
}
