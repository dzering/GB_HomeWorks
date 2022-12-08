﻿using UnityEngine.Networking;
using UnityEngine;
using System.Text;

    public sealed class Client : MonoBehaviour
    {
        public delegate void OnMessageReceive(object message);
        public event OnMessageReceive onMessageReceive;

        private const int MAX_CONNECTIONS = 10;
        private int _port = 0;
        private int _serverPort = 5805;
        private int _hostID;
        private int _reliableChannel;
        private int _connectionID;
        private bool _isConnected = false;
        private byte _error;

        [System.Obsolete]
        public void Connect()
        {
            NetworkTransport.Init();
            ConnectionConfig cc = new ConnectionConfig();
            _reliableChannel = cc.AddChannel(QosType.Reliable);
            HostTopology topology = new HostTopology(cc, MAX_CONNECTIONS);
            _hostID = NetworkTransport.AddHost(topology, _port);
            _connectionID = NetworkTransport.Connect(_hostID, "127.0.0.1", _serverPort, 0, out _error);
            if ((NetworkError)_error == NetworkError.Ok)
                _isConnected = true;
            else
                Debug.Log((NetworkError)_error);
        }

        [System.Obsolete]
        public void Disconnect()
        {
            if (!_isConnected) return;
            NetworkTransport.Disconnect(_hostID, _connectionID, out _error);
            _isConnected = false;
        }

        [System.Obsolete]
        void Update()
        {
            if (!_isConnected) return;
            int recHostId;
            int connectionId;
            int channelId;
            byte[] recBuffer = new byte[1024];
            int bufferSize = 1024;
            int dataSize;
            NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out
            channelId, recBuffer, bufferSize, out dataSize, out _error);
            while (recData != NetworkEventType.Nothing)
            {
                switch (recData)
                {
                    case NetworkEventType.Nothing:
                        break;
                case NetworkEventType.ConnectEvent:
                        onMessageReceive?.Invoke($"You have been connected to server.");
                        Debug.Log($"You have been connected to server.");
                        break;
                    case NetworkEventType.DataEvent:
                        string message = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                        onMessageReceive?.Invoke(message);
                        Debug.Log(message);
                        break;
                    case NetworkEventType.DisconnectEvent:
                        _isConnected = false;
                        onMessageReceive?.Invoke($"You have been disconnected from server.");
                        Debug.Log($"You have been disconnected from server.");
                        break;
                    case NetworkEventType.BroadcastEvent:
                        break;
                }
                recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer,
                bufferSize, out dataSize, out _error);
            }
        }

        [System.Obsolete]
        public void SendMessages(string message)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(message);
            NetworkTransport.Send(_hostID, _connectionID, _reliableChannel, buffer, message.Length *
            sizeof(char), out _error);
            if ((NetworkError)_error != NetworkError.Ok) Debug.Log((NetworkError)_error);
        }
    }


