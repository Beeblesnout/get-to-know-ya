using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public OnDeath DeathEvent;
    public delegate void OnDeath();

    void Awake() 
    {
        DeathEvent += Kill;
    }

    void Start()
    {
        health = maxHealth;
    }

    public void Damage(float damage)
    {
        health -= damage;
        if (health <= 0) DeathEvent.Invoke();
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}
