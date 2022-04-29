using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

public class Netstartthegame : NetMessage
{

    public Netstartthegame()
    {
        code = OpCode.STARTTHEGAME;
    }
    public Netstartthegame(DataStreamReader reader)
    {
        code = OpCode.STARTTHEGAME;
        Deserialize(reader);

    }
    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)code);
       
    }
    public override void Deserialize(DataStreamReader reader)
    {
       
    }
    public override void receivedonserver(NetworkConnection cnn)
    {
        NetUtility.S_STARTTHEGAME?.Invoke(this, cnn);
    }
    public override void receivedonclient()
    {
        NetUtility.C_STARTTHEGAME?.Invoke(this);
    }
}

