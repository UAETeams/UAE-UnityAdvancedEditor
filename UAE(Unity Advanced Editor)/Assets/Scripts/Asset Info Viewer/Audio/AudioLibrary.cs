using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssetsTools.NET.Extra;
using System.IO;
using UAE.ResourceClass;
using System;

namespace AssetsTools.NET
{
    public class AudioLibrary
    {
        public int m_Format;
        public AudioType m_Type;
        public bool m_3D;
        public bool m_UseHardware;

        //version 5
        public int m_LoadType;
        public int m_Channels;
        public int m_Frequency;
        public int m_BitsPerSample;
        public float m_Length;
        public bool m_IsTrackerFormat;
        public int m_SubsoundIndex;
        public bool m_PreloadAudioData;
        public bool m_LoadInBackground;
        public bool m_Legacy3D;
        public AudioCompressionFormat m_CompressionFormat;

        public StreamingInfo m_StreamingInfo;
        public ResourceReader m_AudioData;

        public AudioLibrary ReadAudioFields(AudioLibrary al,AssetTypeValueField basefield)
        {
            try
            {
                al.m_LoadType = basefield["m_LoadType"].AsInt;
                al.m_Channels = basefield["m_Channels"].AsInt;
                al.m_Frequency = basefield["m_Frequency"].AsInt;
                al.m_BitsPerSample = basefield["m_BitsPerSample"].AsInt;
                al.m_Length = basefield["m_Length"].AsFloat;
                al.m_IsTrackerFormat = basefield["m_IsTrackerFormat"].AsBool;
                al.m_SubsoundIndex = basefield["m_SubsoundIndex"].AsInt;
                al.m_PreloadAudioData = basefield["m_PreloadAudioData"].AsBool;
                al.m_LoadInBackground = basefield["m_LoadInBackground"].AsBool;
                al.m_Legacy3D = basefield["m_Legacy3D"].AsBool;
                al.m_CompressionFormat = (AudioCompressionFormat)basefield["m_CompressionFormat"].AsInt;
                al.m_StreamingInfo = new StreamingInfo(basefield["m_Resource"]);

                return al;
            }
            catch(Exception ex)
            {
                Debug.Log(ex.ToString());
                return null;
            }
        }
        public class StreamingInfo
        {
            public string path;
            public long offset;
            public long size;

            public StreamingInfo(AssetTypeValueField m_Resource)
            {
                this.path = m_Resource["m_Source"].AsString;
                this.offset = m_Resource["m_Offset"].AsLong;
                this.size = m_Resource["m_Size"].AsLong;
            }
        }
        public AudioLibrary ReadResource(AudioLibrary al,MemoryStream resourceStream,long offset,long size)
        {
            {
                ResourceReader reader = new ResourceReader(resourceStream,offset, size);
                al.m_AudioData = reader;
                return al;
            }
        }
    }
    public enum AudioType
    {
        UNKNOWN,
        ACC,
        AIFF,
        IT = 10,
        MOD = 12,
        MPEG,
        OGGVORBIS,
        S3M = 17,
        WAV = 20,
        XM,
        XMA,
        VAG,
        AUDIOQUEUE
    }

    public enum AudioCompressionFormat
    {
        PCM,
        Vorbis,
        ADPCM,
        MP3,
        VAG,
        HEVAG,
        XMA,
        AAC,
        GCADPCM,
        ATRAC9
    }
}