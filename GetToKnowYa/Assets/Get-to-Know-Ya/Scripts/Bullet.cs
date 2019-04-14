using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int owner;
    public float damage;
    public float lifetime;
    float startTime;

    void Start() 
    {
        startTime = Time.time;
    }

    void Update()
    {
        if (Time.time - startTime > lifetime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.gameObject.GetComponent<Health>().Damage(damage);
            Destroy(gameObject);
        }
    }
}
