using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

public class NetKeepitalive : NetMessage
{
   public NetKeepitalive()
    {
        code = OpCode.KEEPITALIVE;
    }
    public NetKeepitalive(DataStreamReader reader)
    {
        code = OpCode.KEEPITALIVE;
        Deserialize(reader);

    }
    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)code);
    }

    public override void Deserialize(DataStreamReader reader)
    {

    }
    public override void receivedonclient()
    {
        NetUtility.C_KEEPITALIVE?.Invoke(this);
    }
    public override void receivedonserver(NetworkConnection cnn)
    {
        NetUtility.S_KEEPITALIVE?.Invoke(this, cnn);
    }
}

