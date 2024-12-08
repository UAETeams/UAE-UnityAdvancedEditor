using AssetsTools.NET;
using AssetsTools.NET.Texture;
using AssetsTools.NET.Extra;
using UnityEngine;
using UnityEngine.UI;
using FMOD;
using FMODUnity;
using System.IO;
using System;
using TMPro;
using UAE.ResourceClass;
using System.Text;
using System.Runtime.InteropServices;

public class ViewAudio : MonoBehaviour
{
    public Load load;
    public SelectedFieldInfo selectedFieldInfo;

    public bool isCurrentFunction;

    public AudioSource UniversalSource;

    public ResourceHandle resHandle;

    public void viewAudioClip()
    {
        if (selectedFieldInfo.cell != null)
        {
            ContactInfo info = selectedFieldInfo.cell._contactInfo;
            var manager = new AssetsManager();

            manager.LoadClassPackage(Path.Combine(Application.persistentDataPath, "classdata.tpk"));

            if (info.BundlePath != "")
            {
                manager.LoadClassPackage(Path.Combine(Application.persistentDataPath, "classdata.tpk"));

                var bunfileInst = manager.LoadBundleFile(info.BundlePath, true);


                foreach (AssetBundleDirectoryInfo assetBundleDirectoryInfo in bunfileInst.file.BlockAndDirInfo.DirectoryInfos)
                {
                    string assetsFileName = assetBundleDirectoryInfo.Name;
                    if (!assetsFileName.EndsWith(".resource") && !assetsFileName.EndsWith(".resS"))
                    {
                        var afileInst = manager.LoadAssetsFileFromBundle(bunfileInst, assetBundleDirectoryInfo.Name, false);

                        var afile = afileInst.file;

                        afile.GenerateQuickLookup();

                        if (StaticSettings.unityMetadata != "")
                        {
                            manager.LoadClassDatabaseFromPackage(StaticSettings.unityMetadata);
                        }
                        else
                        {
                            manager.LoadClassDatabaseFromPackage(afile.Metadata.UnityVersion);
                        }

                        if (afileInst.name == info.parentPath)
                        {
                            foreach (AssetFileInfo AFinfo in afileInst.file.AssetInfos)
                            {
                                if (AFinfo.PathId == info.pathID && (AssetClassID)AFinfo.TypeId == info.TypeID)
                                {
                                    AssetTypeValueField atvf = new AssetTypeValueField();
                                    atvf = manager.GetBaseField(afileInst, AFinfo);

                                    AudioLibrary al = new AudioLibrary();
                                    al.ReadAudioFields(al, atvf);
                                    ResourceReader rer = null;
                                    {
                                        {
                                            AudioLibrary.StreamingInfo streamInfo = al.m_StreamingInfo;
                                            if (streamInfo.path != null && streamInfo.path != "")
                                            {
                                                string searchPath = streamInfo.path;
                                                if (searchPath.StartsWith("archive:/"))
                                                    searchPath = searchPath.Substring(9);

                                                searchPath = Path.GetFileName(searchPath);

                                                AssetBundleFile bundle = afileInst.parentBundle.file;

                                                AssetsFileReader reader = bundle.Reader;
                                                AssetBundleDirectoryInfo[] dirInf = bundle.BlockAndDirInfo.DirectoryInfos;
                                                byte[] audioBytes = null;
                                                for (int i = 0; i < dirInf.Length; i++)
                                                {
                                                    AssetBundleDirectoryInfo ABDInfo = dirInf[i];
                                                    if (ABDInfo.Name == searchPath)
                                                    {
                                                        MemoryStream ms = new MemoryStream();
                                                        AssetsFileReader bundleReader = bunfileInst.file.Reader;
                                                        bundleReader.Position = bunfileInst.file.Header.GetFileDataOffset() + ABDInfo.Offset;
                                                        bundleReader.BaseStream.CopyToCompat(ms, ABDInfo.DecompressedSize);
                                                        rer = new ResourceReader(ms, streamInfo.offset, streamInfo.size);
                                                        audioBytes = rer.GetData();
                                                    }
                                                }
                                                if(audioBytes == null)
                                                {
                                                    load.ComplainError("This Bundle does not contain the required directoryInfo for loading the Audio bytes.");
                                                }
                                                else
                                                {
                                                    var wavbytes = ConvertToWav(audioBytes);
                                                    UniversalSource.clip = OpenWavParser.ByteArrayToAudioClip(wavbytes);
                                                }
                                            }
                                            else
                                            {
                                                load.ComplainError("This Audio has no Stream bytes.");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                manager.UnloadAll();
            }
            else if (info.parentPath != "")
            {
                AssetsFileInstance AFinst = manager.LoadAssetsFile(info.parentPath);
                AFinst.file.GenerateQuickLookup();

                if (StaticSettings.unityMetadata != "")
                {
                    manager.LoadClassDatabaseFromPackage(StaticSettings.unityMetadata);
                }
                else
                {
                    manager.LoadClassDatabaseFromPackage(AFinst.file.Metadata.UnityVersion);
                }

                foreach (AssetFileInfo AFinfo in AFinst.file.AssetInfos)
                {
                    if (AFinfo.PathId == info.pathID && (AssetClassID)AFinfo.TypeId == info.TypeID)
                    {
                        AssetTypeValueField atvf = new AssetTypeValueField();
                        atvf = manager.GetBaseField(AFinst, AFinfo);

                        AudioLibrary al = new AudioLibrary();
                        al.ReadAudioFields(al, atvf);
                        {
                            {
                                AudioLibrary.StreamingInfo streamInfo = al.m_StreamingInfo;
                                if (streamInfo.path != null && streamInfo.path != "")
                                {
                                    string searchPath = streamInfo.path;
                                    if (searchPath.StartsWith("archive:/"))
                                        searchPath = searchPath.Substring(9);

                                    searchPath = Path.GetFileName(searchPath);

                                    resHandle.OpenResourceWindow("Name:- " + searchPath);
                                    isCurrentFunction = true;


                                }
                                else
                                {
                                    load.ComplainError("This Audio has no Stream bytes.");
                                }
                            }
                        }
                    }
                }
            }
            manager.UnloadAll();
        }
    }
    public void viewAudioRest(byte[] resourceBytes)
    {
        if (selectedFieldInfo.cell != null)
        {
            ContactInfo info = selectedFieldInfo.cell._contactInfo;
            var manager = new AssetsManager();

            manager.LoadClassPackage(Path.Combine(Application.persistentDataPath, "classdata.tpk"));

            {
                AssetsFileInstance AFinst = manager.LoadAssetsFile(info.parentPath);
                AFinst.file.GenerateQuickLookup();

                if (StaticSettings.unityMetadata != "")
                {
                    manager.LoadClassDatabaseFromPackage(StaticSettings.unityMetadata);
                }
                else
                {
                    manager.LoadClassDatabaseFromPackage(AFinst.file.Metadata.UnityVersion);
                }

                foreach (AssetFileInfo AFinfo in AFinst.file.AssetInfos)
                {
                    if (AFinfo.PathId == info.pathID && (AssetClassID)AFinfo.TypeId == info.TypeID)
                    {
                        AssetTypeValueField atvf = manager.GetBaseField(AFinst, AFinfo);

                        AudioLibrary al = new AudioLibrary();
                        al.ReadAudioFields(al, atvf);
                        {
                            {
                                try
                                {
                                    AudioLibrary.StreamingInfo streamInfo = al.m_StreamingInfo;

                                    AssetsFileReader reader = new AssetsFileReader(new MemoryStream(resourceBytes));

                                    reader.Position = (long)streamInfo.offset;
                                    byte[] audioBytes = reader.ReadBytes((int)streamInfo.size);

                                    var wavbytes = ConvertToWav(audioBytes);
                                    UniversalSource.clip = OpenWavParser.ByteArrayToAudioClip(wavbytes);

                                    isCurrentFunction = false;
                                }
                                catch (Exception ex)
                                {
                                    load.ComplainError("Possibly loaded wrong resource file due to which audio load failed.\nTry Again...");
                                }
                            }
                        }
                    }
                }
            }
            manager.UnloadAll();
        }
    }

    /// <summary>
    /// 'RIFF' ascii
    /// </summary>
    private const uint RiffFourCC = 0x46464952;
    /// <summary>
    /// 'WAVEfmt ' ascii
    /// </summary>
    private const ulong WaveEightCC = 0x20746D6645564157;
    /// <summary>
    /// 'data' ascii
    /// </summary>
    private const uint DataFourCC = 0x61746164;

    public byte[] SoundToWav(Sound sound, AudioLibrary al)
    {
        var result = sound.getFormat(out _, out _, out int channels, out int bits);
        if (result != RESULT.OK)
            return null;
        result = sound.getDefaults(out var frequency, out _);
        if (result != RESULT.OK)
            return null;
        var sampleRate = al.m_Frequency;
        result = sound.getLength(out var length, TIMEUNIT.PCMBYTES);
        if (result != RESULT.OK)
            return null;
        result = sound.@lock(0, length, out var ptr1, out var ptr2, out var len1, out var len2);
        if (result != RESULT.OK)
            return null;
        byte[] buffer = new byte[len1 + 44];
        //添加wav头
        Encoding.UTF8.GetBytes("RIFF").CopyTo(buffer, 0);
        BitConverter.GetBytes(len1 + 36).CopyTo(buffer, 4);
        Encoding.UTF8.GetBytes("WAVEfmt ").CopyTo(buffer, 8);
        BitConverter.GetBytes(16).CopyTo(buffer, 16);
        BitConverter.GetBytes((short)1).CopyTo(buffer, 20);
        BitConverter.GetBytes((short)channels).CopyTo(buffer, 22);
        BitConverter.GetBytes(sampleRate).CopyTo(buffer, 24);
        BitConverter.GetBytes(sampleRate * channels * bits / 8).CopyTo(buffer, 28);
        BitConverter.GetBytes((short)(channels * bits / 8)).CopyTo(buffer, 32);
        BitConverter.GetBytes((short)bits).CopyTo(buffer, 34);
        Encoding.UTF8.GetBytes("data").CopyTo(buffer, 36);
        BitConverter.GetBytes(len1).CopyTo(buffer, 40);
        Marshal.Copy(ptr1, buffer, 44, (int)len1);
        result = sound.unlock(ptr1, ptr2, len1, len2);
        if (result != RESULT.OK)
            return null;
        return buffer;
    }
    private static byte[] ConvertToWav(byte[] fmodData)
    {
        RESULT result = Factory.System_Create(out FMOD.System system);
        if (result != RESULT.OK)
        {
            return null;
        }

        try
        {
            result = system.init(1, INITFLAGS.NORMAL, IntPtr.Zero);
            if (result != RESULT.OK)
            {
                return null;
            }

            CREATESOUNDEXINFO exinfo = new CREATESOUNDEXINFO();
            exinfo.cbsize = Marshal.SizeOf(exinfo);
            exinfo.length = (uint)fmodData.Length;
            result = system.createSound(fmodData, MODE.OPENMEMORY, ref exinfo, out Sound sound);
            if (result != RESULT.OK)
            {
                return null;
            }

            try
            {
                result = sound.getSubSound(0, out Sound subsound);
                if (result != RESULT.OK)
                {
                    return null;
                }

                try
                {
                    result = subsound.getFormat(out SOUND_TYPE type, out SOUND_FORMAT format, out int numChannels, out int bitsPerSample);
                    if (result != RESULT.OK)
                    {
                        return null;
                    }

                    result = subsound.getDefaults(out float frequency, out int priority);
                    if (result != RESULT.OK)
                    {
                        return null;
                    }

                    int sampleRate = (int)frequency;
                    result = subsound.getLength(out uint length, TIMEUNIT.PCMBYTES);
                    if (result != RESULT.OK)
                    {
                        return null;
                    }

                    result = subsound.@lock(0, length, out IntPtr ptr1, out IntPtr ptr2, out uint len1, out uint len2);
                    if (result != RESULT.OK)
                    {
                        return null;
                    }

                    const int WavHeaderLength = 44;
                    int bufferLen = (int)(WavHeaderLength + len1);
                    byte[] buffer = new byte[bufferLen];
                    using (MemoryStream stream = new MemoryStream(buffer))
                    {
                        using BinaryWriter writer = new BinaryWriter(stream);
                        writer.Write(RiffFourCC);
                        writer.Write(36 + len1);
                        writer.Write(WaveEightCC);
                        writer.Write(16);
                        writer.Write((short)1);
                        writer.Write((short)numChannels);
                        writer.Write(sampleRate);
                        writer.Write(sampleRate * numChannels * bitsPerSample / 8);
                        writer.Write((short)(numChannels * bitsPerSample / 8));
                        writer.Write((short)bitsPerSample);
                        writer.Write(DataFourCC);
                        writer.Write(len1);
                    }
                    Marshal.Copy(ptr1, buffer, WavHeaderLength, (int)len1);
                    subsound.unlock(ptr1, ptr2, len1, len2);
                    return buffer;
                }
                finally
                {
                    subsound.release();
                }
            }
            finally
            {
                sound.release();
            }
        }
        finally
        {
            system.release();
        }
    }
}
