using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

public enum OpCode
{
    KEEPITALIVE = 1,
    WELCOME = 2,
    STARTTHEGAME = 3,
    MAKEMOVE = 4,
    RESET = 5
}


public static class NetUtility 
{
    public static void OnData(DataStreamReader stream, NetworkConnection cnn, Server server = null)
    {
        NetMessage msg = null;
        var opCode = (OpCode)stream.ReadByte();
        switch (opCode)
        {
            case OpCode.KEEPITALIVE: msg = new NetKeepitalive(stream); break;
            case OpCode.WELCOME: msg = new NetWelcome(stream); break;
            case OpCode.STARTTHEGAME: msg = new Netstartthegame(stream); break;
            // case OpCode.MAKEMOVE: msg = new Netmakemove(stream); break;
            // case OpCode.RESET: msg = new Netreset(stream); break;
            default:
                Debug.LogError("No OpCode");
                break;
        }

        if (server != null)
            msg.receivedonserver(cnn);
        else
            msg.receivedonclient();
    }
    public static Action<NetMessage> C_KEEPITALIVE;
    public static Action<NetMessage> C_WELCOME;
    public static Action<NetMessage> C_STARTTHEGAME;
    public static Action<NetMessage> C_MAKEMOVE;
    public static Action<NetMessage> C_RESET;
    public static Action<NetMessage, NetworkConnection> S_KEEPITALIVE;
    public static Action<NetMessage, NetworkConnection> S_WELCOME;
    public static Action<NetMessage, NetworkConnection> S_STARTTHEGAME;
    public static Action<NetMessage, NetworkConnection> S_MAKEMOVE;
    public static Action<NetMessage, NetworkConnection> S_RESET;

}
