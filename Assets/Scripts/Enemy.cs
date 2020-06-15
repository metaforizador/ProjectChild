using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float hp = 100;
    public bool alive = true;

    public void TakeDamage(float amount) {
        hp -= amount;

        if (hp <= 0) {
            Die();
        }
    }

    private void Die() {
        hp = 100;
        alive = false;
        Destroy(gameObject); // Destroy for now
    }
}
