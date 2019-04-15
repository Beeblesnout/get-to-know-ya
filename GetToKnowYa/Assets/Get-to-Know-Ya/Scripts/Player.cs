using System.Collections;
using System.Collections.Generic;
using Popcron.Networking;
using UnityEngine;

public class Player : MonoBehaviour
{
    // -= Variables =-
    [Header("Connections")]
    public bool dummy;
    public User user;
    public Health health;

    [Header("Movement")]
    private Move moveComponent;
    private LookAt2D lookAt2DComponent;
    public float fastVelocity;
    public float slowVelocity;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public bool isShooting;
    public float shotRate;
    public ParticleSystem gunFireEffect;
    float lastShotTime;
    Transform target;
    public SpawnEnemies enemySpawner;

    // -= Basic Methods =-
    void Awake() {
        if (!dummy)
        {
            moveComponent = gameObject.GetComponent<Move>();
            lookAt2DComponent = gameObject.GetComponent<LookAt2D>();
        }
    }

    // void Start() {
        // if (Net.IsServer)
    //     {
    //         GetComponent<SpriteRenderer>().color = Color.red;
    //     }
    //     else
    //     {
    //         GetComponent<SpriteRenderer>().color = Color.blue;
    //     }
    // }

    void FixedUpdate() {
        if (!dummy)
        {
            moveComponent.ToFinger(fastVelocity, slowVelocity, false);
            lookAt2DComponent.Cursor();
        }
        else
        {
            if (!target) target = enemySpawner.existingEnemies[Random.Range(0, enemySpawner.existingEnemies.Count)]?.transform;
            if (target) lookAt2DComponent.WorldPoint(target.position);
        }
        
        if (isShooting && !moveComponent.dashing)
            if (Time.time - lastShotTime > shotRate) 
                DoShot();
    }

    // -= Helper Methods =-
    void DoShot()
    {
        lastShotTime = Time.time;
        Rigidbody2D rb2D = 
            Instantiate(bulletPrefab, transform.position + (transform.up * .5f), Quaternion.AngleAxis(90, Vector3.forward))
                .GetComponent<Rigidbody2D>();
        rb2D.AddForce(transform.up * 2.5f, ForceMode2D.Impulse);
        gunFireEffect.Emit(7);
    }
}
