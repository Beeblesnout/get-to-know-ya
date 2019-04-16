using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject[] players;
    private GameObject targetPlayer;

    private int targetPlayerIndex;
    private float moveDistance;
    public float maxSpeed;
    public float speed;
    public float drag;
    
    public float damage;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake()
    {       
        players = GameObject.FindGameObjectsWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        TargetPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetPlayer) TargetPlayer();
        else ChasePlayer();
    }

    private void ChasePlayer()
    {
        Vector2 direction = ((Vector2)targetPlayer.transform.position - rb.position).normalized;
        if (rb.velocity.magnitude < maxSpeed)
            rb.AddForce(direction * speed, ForceMode2D.Force);
        rb.velocity *= 1-drag;
    }

    private void TargetPlayer()
    {
        targetPlayerIndex = Random.Range(0, players.Length);
        targetPlayer = players[targetPlayerIndex];
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        print("boink");
        if (other.gameObject.tag == "Player")
        {
            Health h = other.gameObject.GetComponent<Health>();
            h.Damage(damage);   
        }
    }
}
