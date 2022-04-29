using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class Server : MonoBehaviour
{
    public static Server instance { set; get; }
    private void Awake()
    {
        instance = this;
    }
    public NetworkDriver driver;
    private NativeList<NetworkConnection> connections;

    private bool isWorking = false;
    private const float KeepAliveTickRate = 20.0f;
    private float finalKeepAlive;

    public Action connectionlowered;
    // Methods
    public void Init(ushort port)
    {
        driver = NetworkDriver.Create();
        NetworkEndPoint endpoint = NetworkEndPoint.AnyIpv4;

        endpoint.Port = port;
        if(driver.Bind(endpoint) != 0)
        {
            Debug.Log("Unable to bind on port" + endpoint.Port);
        }
        else
        {
            driver.Listen();
            Debug.Log("Currently listening on port" + endpoint.Port);
        }

        connections = new NativeList<NetworkConnection>(2, Allocator.Persistent);
        isWorking = true;
    }
    public void Shutdown()
    {
        if (isWorking)
        {
            driver.Dispose();
            connections.Dispose();
            isWorking = false;
        }

    
    }
    public void OnDestroy()
    {
        Shutdown();
    }
    public void Update()
    {
        if (!isWorking)
            return;

        Keepitalive();

        driver.ScheduleUpdate().Complete();
        ClearConnections();
        AcceptNew();
        UpdateMsg();
    }
    private void Keepitalive()
    {
        if(Time.time - finalKeepAlive > KeepAliveTickRate)
        {
            finalKeepAlive = Time.time;
            Broadcast(new NetKeepitalive());
        }
    }
    private void ClearConnections()
    {
        for (int i = 0; i < connections.Length; i++ )
        {
            if (!connections[i].IsCreated)
            {
                connections.RemoveAtSwapBack(i);
                --i;
            }
        }
    }
    private void AcceptNew()
    {
        NetworkConnection c;
        while ((c = driver.Accept()) != default(NetworkConnection))
        {
            connections.Add(c);
        }
    }
    private void UpdateMsg()
    {
        DataStreamReader stream;
        for (int i = 0; i < connections.Length; i++)
        {
            NetworkEvent.Type cmd;
            while ((cmd = driver.PopEventForConnection(connections[i], out stream)) != NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Data)
                {
                    NetUtility.OnData(stream, connections[i], this);

                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client has been disconnected");
                    connections[i] = default(NetworkConnection);
                    connectionlowered?.Invoke();
                    Shutdown(); //only happens in a 2 player game
                }
            }
        }
    }

    //Just for the server
    public void Broadcast(NetMessage msg)
    {
        for (int i = 0; i < connections.Length; i++)
            if (connections[i].IsCreated)
            {
               // Debug.Log($"Sending {msg.Code} to : {connections[i].InternalId}");
                    SendToClient(connections[i], msg);
            }
       
    }
    public void SendToClient(NetworkConnection connection, NetMessage msg)
    {
        DataStreamWriter writer;
        driver.BeginSend(connection, out writer);
        msg.Serialize(ref writer);
        driver.EndSend(writer);
    }
    


}
