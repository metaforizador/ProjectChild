using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletController : MonoBehaviour
{
    public CharacterParent.CharacterType shooter { get; private set; }
    public float damage { get; private set; }
    public DamageType damageType { get; private set; }

    public float secondsAlive = 10;
    private float aliveCounter = 0;

    public void Initialize(CharacterParent.CharacterType shooter, float damage, DamageType damageType) {
        this.shooter = shooter;
        this.damage = damage;
        this.damageType = damageType;
    }

    // Update is called once per frame
    void Update()
    {
        aliveCounter += Time.deltaTime;

        if(aliveCounter > secondsAlive)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}
