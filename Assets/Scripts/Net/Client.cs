using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class Client : MonoBehaviour
{
    public static Server instance { set; get; }
    private void Awake()
    {
        // instance = this;
    }
    public NetworkDriver driver;
    private NetworkConnection connection;

    private bool isWorking = false;
    public Action connectionlowered;
    public void Init(string ip, ushort port)
    {
        driver = NetworkDriver.Create();
        NetworkEndPoint endpoint = NetworkEndPoint.Parse(ip, port);

        connection = driver.Connect(endpoint);

        Debug.Log("Attempting to connect to server on" + endpoint.Address);

        isWorking = true;
       // RegisterToEvent();
    }
    public void Shutdown()
    {
        if (isWorking)
        {
            // UnregisterToEvent();
            driver.Dispose();
            connection = default(NetworkConnection);
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

        driver.ScheduleUpdate().Complete();
        CheckifAlive();
        UpdateMsg();
    }
    private void CheckifAlive()
    {
        if(!connection.IsCreated && isWorking)
        {
            Debug.Log("Something went wrong, Lost connection to the server");
            connectionlowered?.Invoke();
            Shutdown();
        }
    }
    
    private void UpdateMsg()
    {
        DataStreamReader stream;
        NetworkEvent.Type cmd;
        while ((cmd = connection.PopEvent(driver, out stream)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                SendTotheServer(new NetWelcome());
                Debug.Log("connected");

            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                 NetUtility.OnData(stream, default(NetworkConnection));
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("Client got disconnected from server");
                connection = default(NetworkConnection);
                connectionlowered?.Invoke();
                Shutdown();
            }
        }
        
    }
    private void SendTotheServer(NetMessage msg)
    {
        DataStreamWriter writer;
        driver.BeginSend(connection, out writer);
        msg.Serialize(ref writer);
        driver.EndSend(writer);
    }
    private void RegisterToEvent()
    {
         NetUtility.C_KEEPITALIVE += OnKeepItAlive;
    }
    private void unregisterToEvent()
    {
        NetUtility.C_KEEPITALIVE += OnKeepItAlive;
    }
    private void OnKeepItAlive(NetMessage nm)
    {
        // Send it back, to keep both side alive
        SendTotheServer(nm);
    }


}
