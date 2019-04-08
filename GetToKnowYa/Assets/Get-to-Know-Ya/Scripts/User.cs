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
    private GameObject p_avatar;
    public GameObject Avatar { 
        get { return p_avatar; }
        private set { 
            p_avatar = value;
            player = p_avatar.GetComponent<Player>();
            player.user = this;
        }
    }
    public Player player { get; private set; }

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
        // else if (IsServer)
        // {
        //     QuestionManager.RequestNewQuestion += GenerateNewQuestion;
        // }
    }

    private async void OnPlayerConnected(NetConnection connection)
    {
        await Task.Delay(50);

        //send alive state
        Message message = new Message(NMType.PlayerAliveState);
        message.Write(ConnectID);
        message.Write(isAlive);
        message.Send(connection);

        if (Avatar)
        {
            //send position
            message = new Message(NMType.PlayerPosition);
            message.Write(ConnectID);
            message.Write(Avatar.transform.position.x);
            message.Write(Avatar.transform.position.y);
            message.Send(connection);
            
            //send rotation
            message = new Message(NMType.PlayerAngle);
            message.Write(ConnectID);
            message.Write(Avatar.transform.eulerAngles.z);
            message.Send(connection);
        }
    }

    /// <summary>
    /// Register information for a remote user, sent from the corrispondant client user.
    /// </summary>
    /// <param name="message">The sent message from the client user, for the remote user</param>
    private void OnMessage(Message message)
    {
        if (IsMine) return;

        NMType messageType = (NMType)message.Type;
        message.Rewind();
        switch (messageType)
        {
            case NMType.ServerNewQuestion:
                int qIndex = message.Read<int>();
                int c1Index = message.Read<int>();
                int c2Index = message.Read<int>();
                QuestionManager.Instance.SetQuestionVars(
                    qIndex, c1Index, c2Index
                );
                break;

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
            // send alive state if it has changed
            if (isAlive != lastAlive)
            {
                lastAlive = isAlive;
                Message message = new Message(NMType.PlayerAliveState);
                message.Write(ConnectID);
                message.Write(isAlive);
                message.Send();
            }

            if (Avatar)
            {
                // send position info if it has changed
                if (Avatar.transform.position != lastAvatarPosition)
                {
                    lastAvatarPosition = Avatar.transform.position;
                    Message message = new Message(NMType.PlayerPosition);
                    message.Write(lastAvatarPosition.x);
                    message.Write(lastAvatarPosition.y);
                    message.Send();
                }

                // send rotation info if it has changed
                if (Avatar.transform.rotation.z != lastAvatarAngle)
                {
                    lastAvatarAngle = Avatar.transform.rotation.z;
                    Message message = new Message(NMType.PlayerAngle);
                    message.Write(ConnectID);
                    message.Write(lastAvatarAngle);
                    message.Send();
                }

                // send shoot state info if it has changed
                if (player.isShooting != lastShooting)
                {
                    lastShooting = player.isShooting;
                    Message message = new Message(NMType.PlayerShootState);
                    message.Write(ConnectID);
                    message.Write(lastShooting);
                    message.Send();
                }
            }
        }
        else
        {
            if (Avatar)
            {
                // set position
                Avatar.transform.position = avatarPosition;
                // set rotation
                Avatar.transform.rotation = Quaternion.AngleAxis(avatarAngle, Vector3.forward);
                // set shooting
                player.isShooting = true;
            }
        }

        if (Avatar == null && isAlive)
        {
            // spawn avatar if alive and there is none
            if (IsMine)
            {
                Avatar = Instantiate(playerPrefab);
                // handle spawn location below

            }
            else
            {
                Avatar = Instantiate(dummyPlayerPrefab);
            }
        }
        else if (Avatar != null && !isAlive)
        {
            // destroy avatar if not alive and there is one
            Destroy(Avatar);
        }
    }

    /// <summary>
    /// Only used by the client. Sends the user's choice.
    /// </summary>
    public void SendChoice(int choiceSelection)
    {
        
    }

    public void FetchNewQuestion()
    {
        
    }

    private async void KillAvatar()
    {
        await Task.Delay(1000);
        Destroy(Avatar);
    }
}
