using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponSO : ScriptableObject {

    public new string name;
    public GameObject weaponPrefab;

    [Header("(Max health and armor is 100)")]
    public float damagePerBullet;
    public DamageType weaponType;
    public float bulletSpeed;
    [Header("Amount of percentage a bullet consumes ammo")]
    public int bulletConsumption;
    [Header("Bullet per second")]
    public float rateOfFire;
}
