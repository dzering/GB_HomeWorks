using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour
{
    private const int MAX_CONNECTIONS = 10;
    private int _port = 5805;
    private int _hostID;
    private int _reliableChannel;
    private bool _isStarted = false;
    private byte error;
    private List<int> _connectionIDs;

    [System.Obsolete]
    private void Update()
    {
        int recHostID;
        int connectionID;
        int channelID;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        NetworkEventType recData = NetworkTransport.Receive(out recHostID, out connectionID,
           out channelID, recBuffer, bufferSize, out dataSize, out error);

        while(recData != NetworkEventType.Nothing)
        {
            switch (recData)
            {
                case NetworkEventType.Nothing:
                    break;
                case NetworkEventType.DataEvent:
                    string message = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    SendMessageAll($"Player {connectionID}: {message}");
                    Debug.Log($"Player {connectionID}: {message}");
                    break;
                case NetworkEventType.ConnectEvent:
                    SendMessageAll($"Player {connectionID} has connected.");
                    Debug.Log($"Player {connectionID} has connected.");
                    break;
                case NetworkEventType.DisconnectEvent:
                    SendMessageAll($"Player {connectionID} has disconnected.");
                    Debug.Log($"Player {connectionID} has disconnected.");

                    break;
                case NetworkEventType.BroadcastEvent:
                    break;
                default:
                    break;
            }
           recData = NetworkTransport.Receive(out recHostID, out connectionID,
           out channelID, recBuffer, bufferSize, out dataSize, out error);
        }

    }

    [System.Obsolete]
    public void StartServer()
    {
        NetworkTransport.Init();
        ConnectionConfig connectionConfig = new ConnectionConfig();
        _reliableChannel = connectionConfig.AddChannel(QosType.Reliable);
        HostTopology hostTopology = new HostTopology(connectionConfig, MAX_CONNECTIONS);
        _hostID = NetworkTransport.AddHost(hostTopology, _port);
        _isStarted = true;
    }

    [System.Obsolete]
    public void ShutDownServer()
    {
        if (!_isStarted)
            return;
        NetworkTransport.RemoveHost(_hostID);
        NetworkTransport.Shutdown();
        _isStarted = false;
    }

    [System.Obsolete]
    public void SendMessege(string message, int connectionID)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(message);
        NetworkTransport.Send(_hostID, connectionID, _reliableChannel,
            buffer, buffer.Length * sizeof(char), out byte error);
        if ((NetworkError)error != NetworkError.Ok)
            Debug.Log((NetworkError)error);
    }

    public void SendMessageAll(string message)
    {
        for (int i = 0; i < _connectionIDs.Count; i++)
        {
            SendMessage(message, _connectionIDs[i]);
        }
    }
}
