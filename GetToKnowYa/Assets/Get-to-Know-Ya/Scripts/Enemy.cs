using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject[] players;
    private GameObject targetPlayer;

    private int targetPlayerIndex;
    private float moveDistance;
    public float speed;

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
        ChasePlayer();
    }

    private void ChasePlayer()
    {
        moveDistance = speed * Time.deltaTime;
        Vector2 direction = ((Vector2)targetPlayer.transform.position - rb.position).normalized;
        if (rb.velocity.magnitude < speed)
            rb.AddForce(direction, ForceMode2D.Force);
    }

    private void TargetPlayer()
    {
        targetPlayerIndex = Random.Range(0, players.Length);
        targetPlayer = players[targetPlayerIndex];
    }
}
