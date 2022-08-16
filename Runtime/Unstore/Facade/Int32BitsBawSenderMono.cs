using Eloi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Int32BitsBawSenderMono : MonoBehaviour
{
    public Int32BitsBawSender m_sender = new Int32BitsBawSender();
    public ListOfBytesEvent m_onNewBytesListToSend;
    public BytesEvent m_onNewBytesToSend;
    [ContextMenu("Se tMax Size As Udp Mod 32 Less One")]
    public void SetMaxSizeAsUdpMod32LessOne()
    {
        m_sender.SetMaxSizeAsUdpMod32LessOne();
    }
    public void TryConvert(in Texture textureToSend, in uint contextId, out bool succed,
       out List<byte[]> textureAsBytes)
    {
        m_sender.Convert(in textureToSend, in contextId, out succed, out textureAsBytes);
        if (succed)
        {
            Push(in textureAsBytes);
        }
    }
    public void TryConvertAndPush( Texture textureToSend, uint contentID)
    {
        m_sender.Convert(in textureToSend, in  contentID, out bool succed, out List<byte[]> representationAffected);
        if (succed)
        {
            Push(in representationAffected);
        }
    }
    private void Push(in List<byte[]> textureAsBytes)
    {
        m_onNewBytesListToSend.Invoke(textureAsBytes);
        for (int i = 0; i < textureAsBytes.Count; i++)
        {
            m_onNewBytesToSend.Invoke(textureAsBytes[i]);
        }
    }

    //[System.Serializable]
    //public class Int32BitsArray2DMultiPackageFullBytesWrapperEvent : UnityEvent<Int32BitsArray2DMultiPackageFullBytesWrapper> { }
    [System.Serializable]
    public class ListOfBytesEvent : UnityEvent<List<byte[]>> { }
    [System.Serializable]
    public class BytesEvent : UnityEvent<byte[]> { }

}


[System.Serializable]
public class Int32BitsBawSender
{
    [Header("Dev Note")]
    public string m_note = "This code is not multi-thread safe";

    public int m_maxPackageByteSize = 64;

    [Header("Compress")]
    public AConvert_Compressed_Texture2Int32BitsArrayMono i_textureToInt32Bits;
    public AConvert_Compressed_Int32BitsArray2PreBytesMono i_int32BitsToPreByte;
    public AConvert_Compressed_PreBytes2FullBytesStorageMono i_preByteToFull;
    public IConvert_Compressed_Texture2Int32BitsArray i0;
    public IConvert_Compressed_Int32BitsArray2PreBytes i1;
    public IConvert_Compressed_PreBytes2FullBytesStorage i2;


    [Header("Data")]
    public TextureSourceToInt32BitsArray2DWrapper m_texture = new TextureSourceToInt32BitsArray2DWrapper();
    public Int32BitsArray2DWrapper m_int32bits = new Int32BitsArray2DWrapper();
    public List<Int32BitsArray2DMultiPackagePreBytesWrapper> m_multiPreBytes = new List<Int32BitsArray2DMultiPackagePreBytesWrapper>();
    public Int32BitsArray2DMultiPackageFullBytesWrapper[] m_multiFullBytes = new Int32BitsArray2DMultiPackageFullBytesWrapper[0];



    public void SetTextureToUse(Texture texture)
    {
        m_texture.m_data.m_textureReference = texture;
    }


    [ContextMenu("Se tMax Size As Udp Mod 32 Less One")]
    public void SetMaxSizeAsUdpMod32LessOne()
    {
        m_maxPackageByteSize = 65407;
    }

    public void Convert(in Texture textureToSend, in uint contextId, out bool succed,
       out List<byte[]> textureAsBytes)
    {
        Convert(in textureToSend, in  contextId, out succed,
            out Int32BitsArray2DMultiPackageFullBytesWrapper[] wrappers);
        textureAsBytes = new List<byte[]>();
        for (int i = 0; i < wrappers.Length; i++)
        {
            textureAsBytes.Add(wrappers[i].m_data.m_compressedInOneBlockOfBytesToStore);
        }
    }
    public void Convert(in Texture textureToSend, in uint contextId, out bool succed,
        out Int32BitsArray2DMultiPackageFullBytesWrapper[] textureAsBytes)
    {
        succed = false;
        textureAsBytes = null;
        try
        {
            i0 = i_textureToInt32Bits;
            i1 = i_int32BitsToPreByte;
            i2 = i_preByteToFull;
            m_texture.m_data.m_contextId.m_contextId = contextId;
            m_texture.m_data.m_textureReference = textureToSend;
            i0.Convert(in this.m_texture, ref this.m_int32bits);
            i1.Convert(in this.m_int32bits, ref this.m_multiPreBytes, in m_maxPackageByteSize);
            m_multiFullBytes = new Int32BitsArray2DMultiPackageFullBytesWrapper[m_multiPreBytes.Count];

            for (int i = 0; i < m_multiPreBytes.Count; i++)
            {
                i2.Convert(m_multiPreBytes[i], ref this.m_multiFullBytes[i]);
            }
            textureAsBytes = m_multiFullBytes;
            succed = true;
        }
        catch (Exception e)
        {
            Debug.LogWarning("Exception:" + e.StackTrace);
            succed = false;
        }
    }
}