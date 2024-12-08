using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Text;
using Unity.Collections;
using System;

public class NetConsoleMessage : NetMessage
{
    public int LogLength;
    public NativeArray<byte> ByteArray;


    public string RecievedLog;

    public NetConsoleMessage()
    {
        Code = OpCode.CONSOLE_LOG;
    }
    public NetConsoleMessage(DataStreamReader stream)
    {
        Code = OpCode.CONSOLE_LOG;

        DeSerialize(stream);
    }
    public NetConsoleMessage(string log)
    {
        Code = OpCode.CONSOLE_LOG;

        byte[] dat = Encoding.ASCII.GetBytes(log);
        ByteArray = new NativeArray<byte>(dat.Length, Allocator.Persistent);
        ByteArray.CopyFrom(dat);
        LogLength = dat.Length;
    }
    public override void Serialize(ref DataStreamWriter streamWriter)
    {
        streamWriter.WriteUInt((uint)Code);
        streamWriter.WriteInt(LogLength);
        streamWriter.WriteBytes(ByteArray);
    }
    public override void DeSerialize(DataStreamReader streamReader)
    {
        int LogLength = streamReader.ReadInt();
        NativeArray<byte> array = new NativeArray<byte>(LogLength, Allocator.Persistent);
        streamReader.ReadBytes(array);
        byte[] dat = new byte[array.Length];
        array.CopyTo(dat);

        RecievedLog = Encoding.ASCII.GetString(dat);
    }
}
