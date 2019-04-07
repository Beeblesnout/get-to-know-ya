using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Popcron.Networking;
using Popcron.Console;
using UnityEngine;

public class User : NetUser
{
    public GameObject playerPrefab;
    public GameObject dummyPlayerPrefab;

    public static User Local { get; private set; }
    public GameObject avatar { 
        get { return avatar; }
        private set { 
            avatar = value;
            avatarPlayer = avatar.GetComponent<Player>();
            avatarPlayer.user = this;
        }
    }
    public Player avatarPlayer { get; private set; }

    public bool isAlive = false;
    private bool lastAlive;
    private Vector3 avatarPosition;
    private Vector3 lastAvatarPosition;
    private float avatarAngle;
    private float lastAvatarAngle;
    private bool shooting;
    private bool lastShooting;

    void OnEnable()
    {
        MultiplayerManager.onMessage += OnMessage;
        Net.PlayerConnectedEvent += OnPlayerConnected;
    }

    void OnDisable() 
    {
        MultiplayerManager.onMessage -= OnMessage;
        Net.PlayerConnectedEvent -= OnPlayerConnected;
    }

    void Start()
    {
        if (IsMine) 
        {
            Local = this;
            isAlive = true;
        }
    }

    private async void OnPlayerConnected(NetConnection connection)
    {
        await Task.Delay(50);

        //send alive state
        Message message = new Message(NMType.PlayerAliveState);
        message.Write(ConnectID);
        message.Write(isAlive);
        message.Send(connection);

        if (avatar)
        {
            //send position
            message = new Message(NMType.PlayerPosition);
            message.Write(ConnectID);
            message.Write(avatar.transform.position.x);
            message.Write(avatar.transform.position.y);
            message.Send(connection);
            
            //send rotation
            message = new Message(NMType.PlayerAngle);
            message.Write(ConnectID);
            message.Write(avatar.transform.eulerAngles.z);
            message.Send(connection);
        }
    }

    private void OnMessage(Message message)
    {
        if (IsMine) return;

        NMType messageType = (NMType)message.Type;
        message.Rewind();
        switch (messageType)
        {
            case NMType.PlayerAliveState:
                if (message.Read<long>() == ConnectID)
                {
                    isAlive = message.Read<bool>();
                }
                break;
            case NMType.PlayerPosition:
                if (message.Read<long>() == ConnectID)
                {
                    float x = message.Read<float>();
                    float y = message.Read<float>();
                    avatarPosition = new Vector3(x, y, 0);
                }
                break;
            case NMType.PlayerAngle:
                if (message.Read<long>() == ConnectID)
                {
                    float a = message.Read<float>();
                    avatarAngle = a;
                }
                break;
            case NMType.PlayerShootState:
                if (message.Read<long>() == ConnectID)
                {
                    shooting = message.Read<bool>();
                }
                break;
            default:
                break;
        }
    }

    void Update()
    {
        if (IsMine)
        {
            if (isAlive != lastAlive)
            {
                lastAlive = isAlive;
                Message message = new Message(NMType.PlayerAliveState);
                message.Write(ConnectID);
                message.Write(isAlive);
                message.Send();
            }

            if (avatar)
            {
                // sync position
                if (avatar.transform.position != lastAvatarPosition)
                {
                    lastAvatarPosition = avatar.transform.position;
                    Message message = new Message(NMType.PlayerPosition);
                    message.Write(lastAvatarPosition.x);
                    message.Write(lastAvatarPosition.y);
                    message.Send();
                }

                // sync rotation
                if (avatar.transform.rotation.z != lastAvatarAngle)
                {
                    lastAvatarAngle = avatar.transform.rotation.z;
                    Message message = new Message(NMType.PlayerAngle);
                    message.Write(ConnectID);
                    message.Write(lastAvatarAngle);
                    message.Send();
                }

                // sync shoot
                if (avatarPlayer.isShooting != lastShooting)
                {
                    lastShooting = avatarPlayer.isShooting;
                    Message message = new Message(NMType.PlayerShootState);
                    message.Write(ConnectID);
                    message.Write(lastShooting);
                    message.Send();
                }
            }
        }
        else
        {
            if (avatar)
            {
                // set position
                avatar.transform.position = avatarPosition;
                // set rotation
                avatar.transform.rotation = Quaternion.AngleAxis(avatarAngle, Vector3.forward);
                // set shooting
                avatarPlayer.isShooting = true;
            }
        }

        if (avatar == null && isAlive)
        {
            if (IsMine)
            {
                avatar = Instantiate(playerPrefab);
                // handle spawn location below

            }
            else
            {
                avatar = Instantiate(dummyPlayerPrefab);
            }
        }
        else if (avatar != null && !isAlive)
        {
            Destroy(avatar);
        }
    }

    private async void KillAvatar()
    {
        await Task.Delay(1000);
        Destroy(avatar);
    }
}
