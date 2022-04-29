using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

public class NetWelcome : NetMessage
{
    public int SetSide { set; get; }

    public NetWelcome()
    {
        code = OpCode.WELCOME;
    }
    public NetWelcome(DataStreamReader reader)
    {
        code = OpCode.WELCOME;
        Deserialize(reader);
         
    }
    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)code);
        writer.WriteInt(SetSide);
    }
    public override void Deserialize(DataStreamReader reader)
    {
        SetSide = reader.ReadInt();
    }
    public override void receivedonserver(NetworkConnection cnn)
    {
        NetUtility.S_WELCOME?.Invoke(this, cnn);
    }
    public override void receivedonclient()
    {
        NetUtility.C_WELCOME?.Invoke(this);
    }
}
