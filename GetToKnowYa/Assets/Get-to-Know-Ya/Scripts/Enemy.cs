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

    // Start is called before the first frame update
    void Awake()
    {       
        players = GameObject.FindGameObjectsWithTag("Player");
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
        transform.position = Vector3.MoveTowards(transform.position, targetPlayer.transform.position, moveDistance);
    }

    private void TargetPlayer()
    {
        targetPlayerIndex = Random.Range(0, players.Length);
        targetPlayer = players[targetPlayerIndex];
    }
}
