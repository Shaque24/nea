using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;
public class NetMessage 
{
    public OpCode code { set; get; }

    public virtual void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)code);
    }
    public virtual void Deserialize(DataStreamReader reader)
    {
        
    }
    public virtual void receivedonclient()
    {
        
    }
    public virtual void receivedonserver(NetworkConnection cnn)
    {
        
    }


}

