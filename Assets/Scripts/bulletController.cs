using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletController : MonoBehaviour
{
    public float secondsAlive = 10;
    private float aliveCounter = 0;

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
