using System.Collections;
using System.Collections.Generic;
using Popcron.Networking;
using UnityEngine;

public class User : NetUser
{
    public static User Local { get; private set; }
    public GameObject controllablePlayerPrefab;
    public GameObject playerPrefab;
    public GameObject avatar { get; private set; }
    public bool isAlive = false;
    private bool lastAlive;

    void OnEnable()
    {
        MultiplayerManager.onMessage += OnMessage;
        Net.PlayerConnectedEvent += OnPlayerConnected;
    }
}
