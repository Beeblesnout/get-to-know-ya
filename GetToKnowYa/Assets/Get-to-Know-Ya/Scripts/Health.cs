using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float health;

    void Update()
    {
        if(health <= 0)
        {
            OnDeath();
        }
    }

    public void Damage(float damage)
    {
        Debug.Log("Ow!");
        health -= damage;
    }

    public void OnDeath()
    {
        Destroy(gameObject);
    }
}
