using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponSO : PickableSO {

    [Header("(Note: starting health and armor is 100, total is 200)")]
    public float damagePerBullet;
    public DamageType weaponType;
    [Header("Meters in second")]
    public float bulletSpeed;
    [Header("Amount of bullets in 1 clip")]
    public int ammoSize;
    [Header("Shoot bullet every {value} second")]
    public float rateOfFire;
    [Header("In seconds, only affects enemies")]
    public float reloadTime;
}
