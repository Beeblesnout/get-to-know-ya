using Popcron.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : SingletonBase<MultiplayerManager> 
{
    public static Action<Message> onMessage;
    public static int maxConnections = 2;

    private void OnEnable()
    {
        Net.NetworkReceiveEvent += OnReceive;
        Net.PlayerConnectedEvent += OnPlayerConnected;
    }

    private void OnDisable()
    {
        Net.NetworkReceiveEvent -= OnReceive;
        Net.PlayerConnectedEvent -= OnPlayerConnected;
    }

    private void OnReceive(NetConnection connection, Message message)
    {
        if (connection.connectId == Net.LocalConnectionID)
        {
            onMessage?.Invoke(message);
        }
        else
        {
            if (Net.IsServer)
            {
                message.Send();
            }
        }
    }

    private void OnPlayerConnected(NetConnection connection) 
    {
        if (Net.Connections.Count >= maxConnections) 
        {
            Net.Disconnect();
            Console.WriteLine("Server already has max connections. You've been booted.", LogType.Exception);
        }
    }
}

public enum NMType
{
    PlayerAliveState, PlayerPosition, PlayerAngle, PlayerShootState
}
