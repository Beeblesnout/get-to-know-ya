using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public UnityEvent OnDeath;

    void Awake() 
    {
        OnDeath.AddListener(Kill);
    }

    void Start()
    {
        health = maxHealth;
    }

    public void Damage(float damage)
    {
        health -= damage;
        if (health <= 0) OnDeath.Invoke();
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}
