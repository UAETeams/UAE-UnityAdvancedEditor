using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Text;
using Unity.Collections;
using System;
using System.Collections;

public class NetLoader : NetMessage
{
    public int length;
    public NativeArray<int> LogLengthArray;
    public NativeArray<NativeArray<byte>> ByteArrayArray;


    public string[] RecievedPaths;

    public NetLoader()
    {
        Code = OpCode.LOAD_FILE;
    }
    public NetLoader(DataStreamReader stream)
    {
        Code = OpCode.LOAD_FILE;

        DeSerialize(stream);
    }
    public NetLoader(string[] log)
    {
        Code = OpCode.LOAD_FILE;
        length = log.Length;
        LogLengthArray = new NativeArray<int>(log.Length, Allocator.Persistent);
        ByteArrayArray = new NativeArray<NativeArray<byte>>(log.Length, Allocator.Persistent);

        for (int i = 0; i < log.Length; i++)
        {
            byte[] dat = Encoding.ASCII.GetBytes(log[i]);
            ByteArrayArray[i] = new NativeArray<byte>(dat.Length, Allocator.Persistent);
            ByteArrayArray[i].CopyFrom(dat);
            LogLengthArray[i] = dat.Length;
        }
    }
    public override void Serialize(ref DataStreamWriter streamWriter)
    {
        streamWriter.WriteUInt((uint)Code);
        streamWriter.WriteInt(length);
        for(int i = 0; i < length; i++)
        {
            streamWriter.WriteInt(LogLengthArray[i]);
            streamWriter.WriteBytes(ByteArrayArray[i]);
        }
    }
    public override void DeSerialize(DataStreamReader streamReader)
    {
        length = streamReader.ReadInt();
        LogLengthArray = new NativeArray<int>(length, Allocator.Persistent);
        ByteArrayArray = new NativeArray<NativeArray<byte>>(length, Allocator.Persistent);

        RecievedPaths = new string[length];

        for (int i = 0; i < length; i++)
        {
            LogLengthArray[i] = streamReader.ReadInt();
            ByteArrayArray[i] = new NativeArray<byte>(LogLengthArray[i],Allocator.Persistent);
            streamReader.ReadBytes(ByteArrayArray[i]);
            byte[] dat = new byte[LogLengthArray[i]];
            ByteArrayArray[i].CopyTo(dat);

            string RecievedLog = Encoding.ASCII.GetString(dat);

            RecievedPaths[i] = RecievedLog;
        }
    }
}
